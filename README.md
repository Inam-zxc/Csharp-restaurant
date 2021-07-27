# Csharp-restaurant

C# API with very basic CRUD functions. (just like a menu book)

🚀 Deploy on Heroku https://csharp-restuarant.herokuapp.com (no home route)

Use this link instead https://csharp-restuarant.herokuapp.com/foods
 
## Build with
- [ASP.NET](asp.net)
- [MongoDB](https://www.mongodb.com/)

## Fuctions
### Users routes
| Path                               | Method   | Access       | Description                                               |
| --------------------------------------- |----------|--------------|-----------------------------------------------------------|
| `/users/login/`           | POST      |    Public     | Login |
| `/users/register/` | POST | Public | Register |

### Foods routes
| Path | Method | Access | Description |
| ------- | ----- | ----- | ----- |
| `/foods?search={search}`| GET | Public | Get all foods detail except reviews |
| `/foods/{id}`| GET | Public | Get food with specific id (including reviews)|
| `/foods/ `| POST | Private | Create food deatail |
| `/foods/{id}`| PUT | Private | Update food with specific id|
| `/foods/{id}`| DELETE | Private | Delete food with specific id|

### Reviews routes
| Path | Method | Access | Description |
| ------- | ----- | ----- | ----- |
| `reviews/food/{id}`| POST | Private | Create food's review|
| `reviews/food/{id}`| PUT | Private | Update food's review|
| `reviews/food/{id}`| DELETE | Private | Delete food's review|
