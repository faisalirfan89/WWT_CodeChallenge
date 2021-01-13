# Moe's Tavern

Hello! Thanks for your interest in WWT, and spending your time on this code exercise.

This project has already been started but some of the code is not implemented. The goal is to fill in the logic gaps to make the tests pass, and hopefully have fun playing with GraphQL!

## What's Included

There are a few things in place for you already:

- `Altair UI`: Moe's Tavern ships with the <a href="https://altair.sirmuel.design/">Altair UI</a> so you can debug GraphQL queries and mutations against the API.
- No need to create data, Moe's Tavern is fully stocked with preloaded beer inventory.

## Requirements

- <a href="https://dotnet.microsoft.com/download/dotnet/5.0">.NET v5.0.0</a> or higher
- <a href="https://code.visualstudio.com/">Visual Studio Code</a> or <a href="https://visualstudio.microsoft.com/vs/">Visual Studio 2019</a>

## Instructions

- Locate the failing tests and implement code to make the tests pass.

## Verifying Results

- Run `bash run-tests.sh` or use <a href="https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019">Visual Studio Test Explorer</a> to see if you've completed the requirements.

## Submitting Results

Please fork this project and implement your solution. Send us a link to your repo, or send us a zip file of your implementation.

## Useful Info

<details>
<Summary>How To</Summary>

- Build - `bash build.sh`
- Run tests - `bash run-tests.sh`
- Start API - `bash start.sh`
- Access AltairUI - https://localhost:5001/ui/altair
</details>

<details>
<Summary>Api Security</Summary>

- Moe's Tavern API will only let authorized clients POST requests.
- Add Key/Value pair to the Request Header 
    - Key `Authorization` and Value `Bearer 87c4705d-f4fe-489f-b4d3-dae2c774c2e7`
</details>

<details>
<Summary>Beer Container Conversions</Summary>

Helpful details when Moe sells beer.

| Size | Half Pint | Pint | Growler | Sixth Barrel | Quarter Barrel | Half Barrel | Barrel |
| ------ | ------ | ------ | ------ | ------ | ------ | ------ | ------ |
**Ounces** | 8 | 16 | 64 | 661 | 992 | 1984 | 3968

</details>

<details>
<Summary>GraphQL Examples</Summary>

### Whats on Tap?

To return all items in Moes' inventory.

```graphql
query WhatsOnTap{
  whatsOnTap{
    barrelage
    name,
    style
    id
  }
}
```

### Find Beer

To find a beer in the inventory by id.

```graphql
query FindBeer{
  findBeer(id: 4){
    barrelage
    id
    name
    style
  }
}
```

### Add Beer

To add a beer to Moes' inventory.

```graphql
mutation AddBeer($beer : AddBeer!){
  addBeer(beer: $beer){
    barrelage
    id
    name
    style
  }
}
```

#### Variables

```json
{
  "beer":  {
    "id" : 10,
    "barrelage" : 10,
    "name": "Duff's Private Reserve",
    "style": "Belgian Tripel"
  }
}
```

### Sold Beer

Moe sold a beer and needs to update his inventory accordingly.

```graphql
mutation SoldBeer($beer: SoldBeer!){
  soldBeer(beer: $beer){
    id,
    barrelage,
    name,
    style
  }
}
```

#### Variables

```json
{
  "beer" : {
    "id" : 1,
    "quantity":  20,
    "container": "PINT"
  }
}
```

### Delete Beer

To remove an inventory item by id.

```graphql
mutation DeleteBeer{
  deleteBeer(id: 1)
}
```

</details>

**Have fun!** Feel free to add dependencies or modify any part of the code base except the tests.

>***Note:*** If you need to look something up, that's absolutely allowed, but don't copy another solution, or collaborate on this problem with others. Normally we encourage teamwork, but today we want to see what you can do!

At WWT we value clean, readable code that passes the requirements. If you can make it efficient and easy to read, even better!
