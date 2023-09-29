using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace BankApplication.Models
{
    public class ViewModel
    { private BankApplication_dbContext db = new BankApplication_dbContext();
        public User user { get; set; }
        public ApplicationForm form { get; set; }
        public List<FileDetail> fileDetail { get; set; }

        public ViewModel getModelData(string username)
        {
            ViewModel model = new ViewModel()
            {
                user = db.Users.FirstOrDefault(el => el.UserName == username),
                form = db.ApplicationForms.FirstOrDefault(el => el.CustemerName == user.UserName),
                fileDetail = db.FileDetails.Where(el => el.ApplicationID == form.ApplicationID).ToList()
            };
            return model;
        }
    }
}