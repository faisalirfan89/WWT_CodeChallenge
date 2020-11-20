using MoesTavern.Api.Model;
using GraphQL.Types;

namespace MoesTavern.Api.Model
{
    public class SoldBeerGraphType : InputObjectGraphType<SoldBeer>
    {
        public SoldBeerGraphType()
        {
            Field("Id", d => d.Id, type: typeof(IntGraphType));
            Field("Quantity", d => d.Quantity, type: typeof(IntGraphType));
            Field("Container", d => d.Container, type: typeof(EnumerationGraphType<ContainerType>));
        }
    }

    public class SoldBeer
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ContainerType Container { get; set; }
    }
}