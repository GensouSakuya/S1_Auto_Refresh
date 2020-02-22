using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleForm
{
    public class UserManager
    {
        public static List<UserInfo> GetUsersFromDb()
        {
            using (var db = new SQLiteDb())
            {
                return db.Set<UserInfo>().ToList();
            }
        }
        public static void DelUserFromDB(string userName,string keeperName)
        {
            using (var db = new SQLiteDb())
            {
                var user = db.Set<UserInfo>().Where(p => p.UserName == userName && p.KeeperName == keeperName).FirstOrDefault();
                if (user == null)
                    return;
                db.Set<UserInfo>().Remove(user);
                db.SaveChanges();
            }
        }
    }
}
