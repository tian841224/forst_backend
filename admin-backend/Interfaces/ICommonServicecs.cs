namespace admin_backend.Interfaces
{
    /// <summary>
    /// 共用方法
    /// </summary>
    public interface ICommonServicecs
    {
        /// <summary>
        /// 取得後台帳號名稱
        /// </summary>
        /// <param name="AdminUserId"></param>
        /// <returns></returns>
        Task<string> GetAdminUserNameAsync(int AdminUserId);
    }
}
