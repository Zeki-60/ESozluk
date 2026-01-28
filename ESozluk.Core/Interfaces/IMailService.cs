using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESozluk.Domain.Interfaces
{
    public interface IMailService
    {
        void SendWelcomeEmail(string toEmail, string userName);
        void SendPasswordResetEmail(string email, string resetToken);

    }
}
