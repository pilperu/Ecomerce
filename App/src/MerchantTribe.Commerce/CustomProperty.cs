
using System;
using MerchantTribe.CommerceDTO.v1;

namespace MerchantTribe.Commerce
{

    public class CustomProperty
    {

        private string _DeveloperId = string.Empty;
        private string _Key = string.Empty;
        private string _Value = string.Empty;

        public string DeveloperId
        {
            get { return _DeveloperId; }
            set { _DeveloperId = value; }
        }
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        public int GetValueAsInt()
        {
            int result = 0;
            if (int.TryParse(this.Value, out result))
            {
                return result;
            }
            return 0;
        }
        public void SetValueAsInt(int value)
        {
            this.Value = value.ToString();
        }
        public CustomProperty()
        {

        }

        public CustomProperty Clone()
        {
            return new CustomProperty(this.DeveloperId, this.Key, this.Value);
        }

        public CustomProperty(string devId, string propertyKey, string propertyValue)
        {
            _DeveloperId = devId;
            _Key = propertyKey;
            _Value = propertyValue;
        }

        public void FromDto(CustomPropertyDTO dto)
        {
            this.DeveloperId = dto.DeveloperId;
            this.Key = dto.Key;
            this.Value = dto.Value;
        }
        public CustomPropertyDTO ToDto()
        {
            CustomPropertyDTO dto = new CustomPropertyDTO();
            dto.Value = this.Value;
            dto.Key = this.Key;
            dto.DeveloperId = this.DeveloperId;

            return dto;
        }

        
    }
}