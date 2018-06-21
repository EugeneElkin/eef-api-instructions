namespace ApiInstructions.DataInstructions.Tests.Mocks
{
    using BaseEntities.Entities;

    public class CountryEntityMock: BaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
