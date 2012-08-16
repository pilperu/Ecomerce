using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MerchantTribeStore.Areas.account.Models
{
    public class SignInViewModel
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Password { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PasswordConfirm { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Mode { get; set; }

        public SignInViewModel()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
            this.PasswordConfirm = string.Empty;
            this.Mode = string.Empty;
        }
    }
}