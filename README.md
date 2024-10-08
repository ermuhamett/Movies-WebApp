# From Zero to Hero: REST APIs in .NET

### What is REST?
- REpresentational State Transfer
- REST is an architectual style for building distributed hypermedia systems.

### About project
- The project is an analogue of IMDB or similar online cinemas. That is, we can perform operations for the movie entity.
- We can also set a rating for the film from 1 to 5 by token and through aggregation methods we get the average value of the film as well as who put a like using the token(JWT).
- The project has 2 branches. The first branch is written standardly in controller-api, and the second in minimal-api. At the bottom is the technology stack:
  - ASP.NET Core 8,
  - PostgreSQL(using docker-compose), Dapper,
  - FluentValidation
  - JWT
  - Refit, Swagger
- Additionally, health and versioning checks have also been added 

### Resource naming and routing
- GET /movie**s**
- GET /movies/id
- GET /movies/id/ratings
- GET /ratings/me
- POST/PUT/DELETE /movies/id/ratings

### HTTP Verbs are meaningful
When we want to specify intent about an action we want to make we are going to use HTTP Verbs to describe that!
- POST - Create (create order, add item in basket, create customer...) 
- GET - Retrieve (give me custmer, customers...)
- PUT - Complete update
- PATCH - Partial update
- DELETE - Delete (delete customer, remove item from basket)

### Using response codes to indicate status
- POST
   - Single resource (/items/id): N/A
   - Collection resource (/items): 201 (Location header) -> link to newly created resource, 202
- GET
   - Single resource (/items/id): 200, 404
   - Collection resource (/items): 200
- PUT
   - Single resource (/items/id): 200, 204, 404
   - Collection resource (/items): 405
- DELETE
   - Single resource (/items/id): 200, 404
   - Collection resource (/items): 405

### Flexible response body options
Json
{
  "Name": "Petar"
}

Accept: application/xml
<xml>

### Understanding Idempotency
- No mather how many time you process a specific request, the result will always be the same on the server.
- POST - Not Idempotent
- GET - Idempotent
- PUT - Idempotent
- DELETE - Idempotent
- HEAD -Idempotent
- OPTIONS - Idempotent
- TRACE - Idempotent

### HATEOAS (Hypermedia as the Engine of Application State)
{
  "departmentId": 10,
  "departmentName": "Administrator",
  "locationId": 1700,
  "managerId": 200,
  "links": [
    {
      "href": "10/employees",
      "rel": "employees",
      "type": "GET"
    }
  ]
}

{
  "account": {
    "account_number": 12345,
    "balance": {
      "currency": "usd",
      "value": 100.00
    }
  },
  "links": [
    {
      "deposits": "/accounts/12345/deposits",
      "withdrawals": "/accounts/12345/withdrawals",
      "transfers": "/accounts/12345/transfers",
      "close-requests": "/accounts/12345/close-requests",
    }
  ]
}

### The different types of errors
There are two categories: 
1. Error
- When the client is sending invalid data -> 400
2. Fault
- There is somethig bad with the server -> 500, the request was valid, but something on the server happened that could not be processed.

### Authentication and Authorization in REST APIs
- Authentication -> Process of verifying of who the user is.
- Authorization -> Process of verifying of what the user can do.
- Issuer -> Who the token is generated from.
- Audience -> Who the token is indented for.
- `{
    "userId": "82a241fb-e454-439c-b0a9-ff31bfad79cb",
    "email": "petar@test.com",
    "customClaims": {
        "admin": true,
        "trusted_member": true
    }
  }`

### Why partial updates (PATCH) are not used?
- Complex to build and process the patch request.
- You need to have a path to the item
- Simpler to Get the item, change it and use PUT.

### What is Swagger?
- Is a way for describing language agnostic REST apis and it is all about specification.
- It allows both humans and computers to understand the capabilities of apis in terms of what it can or cannot do and how the contracts look like using standardized format.

Goals:
- Minimize the time needed to document an api
- Easier to integrate with api without need to be connected with the team implementing the api

### Response caching
- Idea that to stop the client sending traffic to the server for no reason if something hasn't changed or isn't due to change in given period of time, it can cache that value and not call the server saving the server resources.
- Response caching is completly based on the client not the server.
- Not controller cache, mainly extractions

### Output caching
- Output caching is actually caching the output the api is returning based on some conditions.
- Purly on server side.
- By default is in memory cache.
- By default only 200 OK is cached.
- By default only GET and HEAD request are cached.
- Responses that set cookies are not cached.
- Responses to auth requests are not cached.

- The most challenging task is cache invalidation.
- **Tag** allows us to invalidate cached entries.
