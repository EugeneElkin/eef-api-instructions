# EEF Apps. API Instructions

A library that contains instructions to cover the main CRUD operations with filtering, grouping, sorting, pagination, etc.

The library helps to quickly build ASP.NET WebAPI applications based on Entity Framework.
Instructions take care of overheads and designed to avoid common mistakes related with correct using of Entity Framework.

## Getting Started

### Installing

To use this library in your VS project, you need to install 2 nuget packages:
- **eef-api-instructions** -- contains instructions to work with SQL requests via Entity Framework Core 2
- **eef-api-instructions-base-entities** -- contains base entities that is used by instructions and must be inherited by your entities

## How to use
It is proposed to use the library in controllers.

### Get list of entities

```
// GET: api/Categories
// Simple list of categoris with pagination and sorting
[HttpGet]
public async Task<IActionResult> GetCategories(string orderByField = null, bool isDescending = false, int? pageSize = null, int? pageAt = null)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // Here is example of calling the instruction to receive a list of categories
    // You can see that controller provides many parameters that may require pagination and sorting
    // The instruction cover all these needs and in fact you only need to specify entity type
    var categories = await new ReceivingListInstruction<Category>(this.context,
        new ListInstructionParams<Category>
        {
            orderByField = orderByField,
            isDescending = isDescending,
            pageAt = pageAt,
            pageSize = pageSize
        }).Execute();

    var sanitizedCategories = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories);

    return Ok(sanitizedCategories);
}
```

### Get list of entities with grouping and filtration
```
// GET api/Tourists/{touristId}/Countries
// Here we wnat to get a list of countries that were visited by a tourist (in certain year)
[HttpGet("{id}/Countries")]
public async Task<IActionResult> GetTouristCountries(string id, string orderByField = null, bool isDescending = false, int? pageSize = null, int? pageAt = null, bool grouped = false, int? year = null)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
    }

    // This example is little bit complicated so, we create parameters before passing them to an instruction
    // You can see that we added filter expression and grouping expression
    // Also we added a nvaigation property to attach countries' details for target tourist
    var instructionParams = new GroupedListInstructionParams<Visit>
    {
        orderByField = orderByField,
        isDescending = isDescending,
        pageAt = pageAt,
        pageSize = pageSize,
        filterExpr = visit => visit.TouristId == id,
        navigationProperties = new string[] { "Country" },
        groupExpr = visit => visit.Country.Name
    };

    // We can adjust filter expression before passing it to an instruction
    if (year.HasValue && year > 1000 && year < 9999)
    {
        instructionParams.filterExpr = visit => visit.TouristId == id && visit.DateOfVisit.Year == year;
    }

    // So, if we need to group our result we will call grouped instruction and group expression will be used
    if (grouped)
    {
        var groupedVisits = await new ReceivingGroupedListInstruction<Visit, string>(this.context, instructionParams).Execute();
        var sanitizedGroupedVisits = Mapper.Map<IEnumerable<GroupedItem>, IEnumerable<TouristCountriesViewModel>>(groupedVisits);
        return Ok(sanitizedGroupedVisits);
    }

    // If we don't need any grouping we will call list instruction without grouping
    // In this case grouping expression, that we proivded in parameters, will be ignored
    var visits = await new ReceivingListInstruction<Visit, string>(this.context, instructionParams).Execute();
    var sanitizedVisits = Mapper.Map<IEnumerable<Visit>, IEnumerable<VisitViewModel>>(visits);
    return Ok(sanitizedVisits);
}

```

### Get an certain entity record

### Create a new entity

### Remove a certain entity record

### Update a certain entity record

### Patch a certain entity record