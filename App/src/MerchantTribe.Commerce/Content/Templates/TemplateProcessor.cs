using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates
{
    public class TemplateProcessor
    {
        public string Template { get; set; }
        private MerchantTribeApplication MTApp;
        Dictionary<string, ITagHandler> Handlers { get; set; }

        public TemplateProcessor(MerchantTribeApplication app, string template)
        {
            this.Handlers = new Dictionary<string, ITagHandler>();
            InitializeTagHandlers();
            this.MTApp = app;
            this.Template = template;
        }
        public TemplateProcessor(MerchantTribeApplication app, string template, Dictionary<string, ITagHandler> handlers)
        {
            this.Handlers = handlers;
            this.MTApp = app;
            this.Template = template;
        }
        private void InitializeTagHandlers()
        {
            AddHandler(this.Handlers, new TagHandlers.Include());
            AddHandler(this.Handlers, new TagHandlers.PageTitle());
            AddHandler(this.Handlers, new TagHandlers.MetaTags());
            AddHandler(this.Handlers, new TagHandlers.Css());
            AddHandler(this.Handlers, new TagHandlers.JavaScript());
            AddHandler(this.Handlers, new TagHandlers.AnalyticsBottom());
            AddHandler(this.Handlers, new TagHandlers.AnalyticsTop());
            AddHandler(this.Handlers, new TagHandlers.TempMessages());
            AddHandler(this.Handlers, new TagHandlers.AdminPanel());
            AddHandler(this.Handlers, new TagHandlers.Area());
            AddHandler(this.Handlers, new TagHandlers.PromoTag());
            AddHandler(this.Handlers, new TagHandlers.Copyright());
            AddHandler(this.Handlers, new TagHandlers.Logo());
            AddHandler(this.Handlers, new TagHandlers.Link());
            AddHandler(this.Handlers, new TagHandlers.SearchForm());
            AddHandler(this.Handlers, new TagHandlers.MainMenu());
            AddHandler(this.Handlers, new TagHandlers.PageMenu());
        }
        private void AddHandler(Dictionary<string, ITagHandler> handlers, ITagHandler handler)
        {
            handlers.Add(handler.TagName, handler);
        }

        public Queue<string> Tokenize()
        {
            Queue<string> result = new Queue<string>();

            bool in_tag = false;
            StringBuilder sb = new StringBuilder();

            foreach (char c in this.Template.ToCharArray())
            {

                if (in_tag == false)
                {
                    if (c == '<')
                    {
                        // starting html tag 
                        // dump any plaintext
                        if (sb.ToString().Length > 0)
                        {
                            result.Enqueue(sb.ToString());
                        }
                        sb = new StringBuilder();
                        in_tag = true;
                        sb.Append(c);
                    }
                    else
                    {
                        // plain text only
                        sb.Append(c);
                    }
                }
                else
                {
                    // searching for ending char
                    if (c == '>')
                    {
                        sb.Append(c);
                        if (sb.ToString().Length > 0)
                        {
                            result.Enqueue(sb.ToString());
                        }
                        sb = new StringBuilder();
                        in_tag = false;
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }

            if (sb.ToString().Length > 0)
            {
                result.Enqueue(sb.ToString());
            }
            return result;
        }

        public string RenderForDisplay()
        {
            return RenderForDisplay(Tokenize());
        }
        public string RenderForDisplay(Queue<string> tokens)
        {
            StringBuilder sb = new StringBuilder();
            ProcessTemplate(tokens, sb);
            return sb.ToString();
        }

        private void ProcessTemplate(Queue<string> tokens, StringBuilder sb)
        {
            string tagStarter = "<";

            bool parsingTag = false;
            Queue<string> subqueue = new Queue<string>();
            string startToken = string.Empty;

            while (tokens.Count > 0)
            {
                string currentToken = tokens.Dequeue();

                if (parsingTag)
                {
                    if (IsClosingTag(currentToken, ParseTagName(startToken)))
                    {
                        // Yes, this tag closed the starting tag
                        ParsedTag t = ParseTag(startToken);
                        ProcessTag(t, subqueue, sb);

                        // reset everything since the tag is parsed below
                        parsingTag = false;
                        startToken = "";
                        subqueue = new Queue<string>();

                    }
                    else
                    {
                        // Nope, no closing yet, just enqueue the token
                        subqueue.Enqueue(currentToken);
                    }
                }
                else
                {
                    // we're not parsing, check for a tag start
                    if (currentToken.StartsWith(tagStarter))
                    {
                        if (IsAcceptedTag(currentToken))
                        {
                            parsingTag = true;

                            if (IsSelfClosed(currentToken))
                            {
                                // Tag is self closed, just parse it
                                ParsedTag t2 = ParseTag(currentToken);
                                ProcessTag(t2, subqueue, sb);

                                parsingTag = false;
                            }
                            else
                            {
                                // store the starting token for later parsing
                                startToken = currentToken;
                            }
                        }
                        else
                        {
                            // not an accepted tag, just dump the sucker
                            sb.Append(currentToken);
                        }
                    }
                    else
                    {
                        // not starting a tag, just dump the output
                        sb.Append(currentToken);
                    }
                }
            }


            if (parsingTag)
            {
                if (startToken.Length > 0)
                {
                    sb.Append(startToken);
                }
            }

            // if the subque has items in it because we didn't find a closing tag, dump them
            if (subqueue.Count > 0)
            {
                while (subqueue.Count > 0)
                {
                    string subqueuetoken = subqueue.Dequeue();
                    sb.Append(subqueuetoken);
                }
            }


        }
        private void ProcessTag(ParsedTag tag, Queue<string> contents, StringBuilder sb)
        {
            if (Handlers.ContainsKey(tag.TagName))
            {
                ITagHandler handler = Handlers[tag.TagName];
                if (handler != null)
                {
                    string contentsFlat = string.Empty;
                    foreach (string s in contents)
                    {
                        contentsFlat += s;
                    }
                    sb.Append(handler.Process(this.MTApp, Handlers, tag, contentsFlat));
                }
            }
        }
        private bool IsClosingTag(string currentToken, string forTagName)
        {
            if (currentToken == "</" + forTagName + ">" ||
                currentToken == "</ " + forTagName + ">")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private ParsedTag ParseTag(string token)
        {
            ParsedTag t = new ParsedTag();
            t.TagName = ParseTagName(token);
            ParseTagAttributes(t, token);
            return t;
        }
        private string ParseTagName(string tag)
        {
            string result = string.Empty;

            string temp = tag.TrimStart('<');
            temp = temp.TrimEnd('>');
            if (temp.EndsWith("/")) temp = temp.TrimEnd('/');

            // temp is now a raw tag without open/close brackets
            string[] parts = temp.Split(' ');
            if (parts != null)
            {
                if (parts.Count() > 0)
                {
                    result = parts[0];
                }
            }

            return result;
        }
        private class TempAttr
        {
            public string _Name = string.Empty;
            public string Name
            {
                get { return _Name; }
                set { _Name = value.ToLowerInvariant(); }
            }
            public string Value { get; set; }

            public TempAttr()
            {
                Name = string.Empty;
                Value = string.Empty;
            }
            public TempAttr(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
        private void ParseTagAttributes(ParsedTag t, string input)
        {

            string tagname = ParseTagName(input);
            string temp = input.TrimStart('<');
            temp = temp.TrimEnd('>');
            temp = temp.TrimEnd('/');
            //Trim off tag name
            temp = temp.Substring(tagname.Length, temp.Length - (tagname.Length));
            temp = temp.Trim();

            bool isParsingAttribute = false;
            bool isParsingAttributeValue = false;

            TempAttr currentAttribute = null;

            // loop through all characters, splitting of attributes
            char[] characters = temp.ToCharArray();
            for (int i = 0; i < temp.Length; i++)
            {
                char current = temp[i];

                if (isParsingAttribute)
                {
                    if (isParsingAttributeValue)
                    {
                        // append the current character
                        currentAttribute.Value += current;

                        // check to see if we're done with the attribute
                        if (currentAttribute.Value.Length >= 2)
                        {
                            if (currentAttribute.Value.EndsWith("\""))
                            {
                                isParsingAttributeValue = false;
                                isParsingAttribute = false;

                                currentAttribute.Value = currentAttribute.Value.TrimStart('"');
                                currentAttribute.Value = currentAttribute.Value.TrimEnd('"');

                                // Add or overwrite value
                                t.SetSafeAttribute(currentAttribute.Name, currentAttribute.Value);

                                currentAttribute = null;
                            }
                        }
                    }
                    else
                    {
                        // we're not parsing the value yet so check for "="
                        if (current == '=')
                        {
                            // skip this charater but enable attribute value parsing;
                            isParsingAttributeValue = true;
                        }
                        else
                        {
                            currentAttribute.Name += current;
                        }
                    }
                }
                else
                {
                    // not parsing right now, check to see if we need to start
                    if (!char.IsWhiteSpace(current))
                    {
                        // not white space so let's start our attribute name
                        currentAttribute = new TempAttr(current.ToString(), "");
                        isParsingAttribute = true;
                    }

                }

            }
        }
        private bool IsAcceptedTag(string currentToken)
        {
            string name = ParseTagName(currentToken);
            if (this.Handlers.ContainsKey(name.ToLowerInvariant()))
            {
                // simple check to make sure tag is closed
                if (currentToken.EndsWith(">") || currentToken.EndsWith("/>"))
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsSelfClosed(string currentToken)
        {
            if (currentToken.EndsWith("/>"))
            {
                return true;
            }
            return false;
        }

    }
}
