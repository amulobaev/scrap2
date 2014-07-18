using Zlatmet2.Domain.Dto;

namespace Zlatmet2.Domain.Tools
{
    internal static class Extensions
    {
        internal static string GetTable(this BaseDto dto)
        {
            return QueryObject.GetTable(dto);
        }

        internal static string InsertQuery(this BaseDto dto)
        {
            return QueryObject.CreateQuery(dto);
        }

        internal static string SelectAllQuery(this BaseDto dto)
        {
            return QueryObject.GetAllQuery(dto);
        }

        internal static string SelectByIdQuery(this BaseDto dto)
        {
            return QueryObject.GetByIdQuery(dto);
        }

        internal static string UpdateQuery(this BaseDto dto)
        {
            return QueryObject.UpdateQuery(dto);
        }

        internal static string DeleteQuery(this BaseDto dto)
        {
            return QueryObject.DeleteQuery(dto);
        }
    }
}