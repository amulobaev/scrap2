using Scrap.Domain.Dto;

namespace Scrap.Domain.Tools
{
    public static class Extensions
    {
        public static string GetTable(this BaseDto dto)
        {
            return QueryObject.GetTable(dto);
        }

        public static string InsertQuery(this BaseDto dto)
        {
            return QueryObject.CreateQuery(dto);
        }

        public static string SelectAllQuery(this BaseDto dto)
        {
            return QueryObject.GetAllQuery(dto);
        }

        public static string SelectByIdQuery(this BaseDto dto)
        {
            return QueryObject.GetByIdQuery(dto);
        }

        public static string UpdateQuery(this BaseDto dto)
        {
            return QueryObject.UpdateQuery(dto);
        }

        public static string DeleteQuery(this BaseDto dto)
        {
            return QueryObject.DeleteQuery(dto);
        }
    }
}