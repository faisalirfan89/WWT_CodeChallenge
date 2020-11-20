using MoesTavern.Data.Model;
using GraphQL.Types;

namespace MoesTavern.Api.Model
{
    public class BeerGraphType : ObjectGraphType<Beer>
    {
        public BeerGraphType()
        {
            Field("Id", d => d.Id, type: typeof(IntGraphType));
            Field("Barrelage", d => d.Barrelage, type: typeof(DecimalGraphType));
            Field("Name", d => d.Name, type: typeof(StringGraphType));
            Field("Style", d => d.Style, type: typeof(StringGraphType));
        }
    }
}