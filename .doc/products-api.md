[Back to README](../README.md)

### Products

#### GET /products
```
  bash
curl -X 'GET' \
  'https://localhost:8081/api/Products?Name=teste&PageNumber=1&PageSize=10' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e'
```
- Response: 
  ```json
  {
    "data": {
      "data": [
        {
          "id": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
          "name": "teste",
          "description": "teste",
          "price": 10
        }
      ],
      "success": true,
      "message": "Products retrieved successfully",
      "errors": []
    },
    "success": true,
    "message": "",
    "errors": []
  }
  ```

#### POST /products
- Description: Add a new product
- Request Body:
  ```bash
  curl -X 'POST' \
  'https://localhost:8081/api/Products' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "teste",
  "description": "teste",
  "price": 10
  }'
  ```
- Response: 
  ```json
  {
    "data": {
      "id": "f04ae628-2f31-463b-b517-1eeb548673a1",
      "name": "teste",
      "description": "teste",
      "price": 10
    },
    "success": true,
    "message": "Product created successfully",
    "errors": []
  }
  ```

<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./auth-api.md">Previous: Auth API</a>
  <a href="./carts-api.md">Next: Carts API</a>
</div>