using System.Collections.Generic;
using System.Linq;

namespace SimpleForm
{
    public class UserManager
    {
        public static List<User> GetUsersFromDb()
        {
            using (var db = new SQLiteDb())
            {
                return db.Set<User>().ToList();
            }
        }

        public static void DelUserFromDB(string userName,string keeperName)
        {
            using (var db = new SQLiteDb())
            {
                var user = db.Set<User>().Where(p => p.UserName == userName && p.KeeperName == keeperName).FirstOrDefault();
                if (user == null)
                    return;
                db.Set<User>().Remove(user);
                db.SaveChanges();
            }
        }

        public static void AddUserToDB(User user)
        {
            using (var db = new SQLiteDb())
            {
                db.Set<User>().Add(user);
                db.SaveChanges();
            }
        }
    }
}
