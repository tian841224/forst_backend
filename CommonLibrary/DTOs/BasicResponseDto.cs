namespace CommonLibrary.DTOs
{
    public class BasicResponseDto
    {
        public int Id { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
    }
}
