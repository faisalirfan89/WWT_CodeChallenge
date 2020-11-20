using GraphQL.Types;
using MoesTavern.Data.Model;

namespace MoesTavern.Api.Model
{
    public class AddBeerGraphType : InputObjectGraphType<Beer>
    {
        public AddBeerGraphType()
        {
            Field("Id", d => d.Id, type: typeof(IntGraphType));
            Field("Barrelage", d => d.Barrelage, type: typeof(DecimalGraphType));
            Field("Name", d => d.Name, type: typeof(StringGraphType));
            Field("Style", d => d.Style, type: typeof(StringGraphType));
        }
    }
}