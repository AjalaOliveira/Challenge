using Challenge.Business.Notifications;
using System.Collections.Generic;

namespace Challenge.Business.Interfaces
{
    public interface INotificator
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}