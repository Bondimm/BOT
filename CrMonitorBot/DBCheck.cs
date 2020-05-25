using CrMonitorBot.Logic;

namespace CrMonitorBot
{
    class DBCheck
    {
        public int DBIndex(UserList user, int id)
        {
            int Index = 0;
            string x = id.ToString();
            for (int i = 0; i < user.users.Count; i++)
            {
                if (user.users[i].Id == x)
                {
                    Index = i;
                }
            }
            return Index;
        }
    }
}
