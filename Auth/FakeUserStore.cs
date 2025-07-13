
namespace SampleJwtApi.Auth
{
    public static class FakeUserStore
    {
        public static readonly List<(string Username, string Password, List<string> Roles, Dictionary<string, string> Claims)> Users =
            new List<(string, string, List<string>, Dictionary<string, string>)>
            {
                ("admin", "password", new List<string> { "Admin" }, new Dictionary<string, string> { { "Department", "HR" } }),
                ("manager", "1234", new List<string> { "Manager" }, new Dictionary<string, string> { { "Department", "Finance" } }),
                ("employee", "pass", new List<string> { "Employee" }, new Dictionary<string, string> { { "Department", "IT" } }),
            };
    }
}
