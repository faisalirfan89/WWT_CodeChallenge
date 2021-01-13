using GraphQL.Types;

namespace MoesTavern.Api.Schema
{
    public interface IInventorySchema : ISchema
    { }
    public class InventorySchema : GraphQL.Types.Schema, IInventorySchema
    {
        public InventorySchema(IInventoryQuery query, IInventoryMutation mutation)
        {
            Query = query;
            Mutation = mutation;
        }
    }
}