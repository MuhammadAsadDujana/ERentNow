using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.NotificationService
{
    public class NotificationService
    {
        eRentEntities _db;
        public NotificationService()
        {
            _db = new eRentEntities();
        }

        public List<Notification> InsertAllNotifications(List<Notification> entity)
        {
            if (entity.Count <= 0)
                return null;

            _db.Notifications.AddRange(entity);
            _db.SaveChanges();
            return entity;
        }
    }
}
