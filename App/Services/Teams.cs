﻿using System;
using System.Text;

namespace Kandu.Services
{
    public class Teams : Service
    {
        public Teams(Core KanduCore) : base(KanduCore)
        {
        }

        public string Create(string name, string description = "")
        {
            if (!CheckSecurity()) { return AccessDenied(); } //check security
            try
            {
                var query = new Query.Teams(S.Server.sqlConnectionString);
                query.CreateTeam(new Query.Models.Team()
                {
                    name = name,
                    description = description,
                    ownerId = S.User.userId,
                    website = "",
                    security = true
                });
            }
            catch(Exception ex)
            {
                return Error();
            }
            return Success();
        }

        public string List()
        {
            if (!CheckSecurity()) { return AccessDenied(); } //check security
            var query = new Query.Teams(S.Server.sqlConnectionString);
            var list = query.GetList(S.User.userId);
            var html = new StringBuilder("{\"teams\":[");
            var i = 0;
            list.ForEach((Query.Models.Team t) =>
            {
                html.Append((i > 0 ? "," : "") + "{\"name\":\"" + t.name + "\", \"description\":\"" + t.description + "\",\"teamId\":\"" + t.teamId + "\"}");
                i++;
            });
            html.Append("]}");
            return html.ToString();
        }
    }
}
