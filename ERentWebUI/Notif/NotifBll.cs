using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERentWebUI.Notif
{
    public class NotifBll
    {
        static readonly string connString = ConfigurationManager.ConnectionStrings["eRentCs"].ConnectionString;

        internal static SqlCommand command = null;
        internal static SqlDependency dependency = null;


        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <returns></returns>
        public static SqlDataReader GetNotification()
        {
            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (command = new SqlCommand(@"SELECT [FirstName],[LastName],[Image],[DOB] FROM [dbo].[Users]", connection))
                    {
                        command.Notification = null;

                        if (dependency == null)
                        {
                            dependency = new SqlDependency(command);
                            dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);
                        }
                        var reader = command.ExecuteReader();
                        return reader;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (dependency != null)
            {
                dependency.OnChange -= dependency_OnChange;
                dependency = null;
            }
            if (e.Type == SqlNotificationType.Change)
            {
                NotifHub.Send();

            }
        }


    }
}