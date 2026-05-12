using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Constants;

public static class Permissions
{
    const string Type = "Permission";
    public static class Roles
    {
        const string View = "Permissions.Roles.View";
        const string Manage = "Permissions.Roles.Manage";
    }

    public static class Users
    {
        const string View = "Permissions.Users.View";
        const string Create = "Permissions.Users.Create";
        const string Delete = "Permissions.Users.Delete";
        const string Edit = "Permissions.Users.Edit";

    }

    public static class Ticket
    {
        const string View = "Permissions.Tickets.View";
        const string Create = "Permissions.Tickets.Create";
    }
    public static class Watchlist
    {
        const string View = "Permissions.Watchlist.View";
        const string Create = "Permissions.Watchlist.Create";
        const string Delete = "Permissions.Watchlist.Delete";
    }
    public static class Screening
    {
        const string View = "Permissions.Screenings.View";
        const string Create = "Permissions.Screenings.Create";
        const string Delete = "Permissions.Screenings.Delete";
        const string Edit = "Permissions.Screenings.Edit";
    }
    public static class Movie
    {
        const string View = "Permissions.Movie.View";
        const string Create = "Permissions.Movie.Create";
        const string Delete = "Permissions.Movie.Delete";
        const string Edit = "Permissions.Movie.Edit";
    }

    public static class Cinema
    {
        const string View = "Permissions.Cinema.View";
        const string Create = "Permissions.Cinema.Create";
        const string Delete = "Permissions.Cinema.Delete";
        const string Edit = "Permissions.Cinema.Edit";
    }

    public static List<string> GetValues()
    {
        return new List<string>
        {
           "Permissions.Roles.Manage",
           "Permissions.Roles.View",
           "Permissions.Users.View",
           "Permissions.Users.Create",
           "Permissions.Users.Delete",
           "Permissions.Users.Edit",
           "Permissions.Tickets.View",
           "Permissions.Tickets.Create",
           "Permissions.Watchlist.View",
           "Permissions.Watchlist.Create",
           "Permissions.Watchlist.Delete",
           "Permissions.Screenings.View",
           "Permissions.Screenings.Create",
           "Permissions.Screenings.Delete",
           "Permissions.Screenings.Edit",
           "Permissions.Movie.View",
           "Permissions.Movie.Create",
           "Permissions.Movie.Delete",
           "Permissions.Movie.Edit",
           "Permissions.Cinema.View",
           "Permissions.Cinema.Create",
           "Permissions.Cinema.Delete",
           "Permissions.Cinema.Edit"
        };

  
    }

}
