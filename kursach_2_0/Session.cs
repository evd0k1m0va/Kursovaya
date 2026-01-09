namespace kursach_2_0
{
    /// <summary>
    /// Глобальная сессия текущего пользователя.
    /// Нужна для: разграничения прав, смены пароля, выхода из профиля.
    /// </summary>
    public static class Session
    {
        public static int UserId { get; set; }
        public static string Login { get; set; } = "";
        public static string Role { get; set; } = "user";          // admin/user
        public static string Permissions { get; set; } = "0000";   // 0000..1111

        public static void Clear()
        {
            UserId = 0;
            Login = "";
            Role = "user";
            Permissions = "0000";
        }
    }
}
