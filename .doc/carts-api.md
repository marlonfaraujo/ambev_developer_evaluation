[Back to README](../README.md)

### Carts

#### GET /carts
```
  bash
curl -X 'GET' \
  'https://localhost:8081/api/Carts' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e'
```
- Response: 
  ```json
  {
  "data": {
    "data": {
      "userId": "40515b29-e112-4457-9a69-c170b833986d",
      "branchSaleId": "035f4be0-ddec-42d5-bde7-6d45b4d39fb8",
      "totalSalePrice": 96,
      "saleItems": [
        {
          "productId": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
          "productItemQuantity": 12,
          "unitProductItemPrice": 10,
          "discountAmount": 0,
          "totalSaleItemPrice": 0,
          "totalWithoutDiscount": 0,
          "saleItemStatus": "Created",
          "id": "00000000-0000-0000-0000-000000000000"
        }
      ]
    },
    "success": true,
    "message": "cart retrieved successfully",
    "errors": []
  },
  "success": true,
  "message": "",
  "errors": []
  }
  ```

#### POST /carts
- Description: Add a new cart
- Request Body:
  ```bash
  curl -X 'POST' \
  'https://localhost:8081/api/Carts' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e' \
  -H 'Content-Type: application/json' \
  -d '{
  "branchSaleId": "035f4be0-ddec-42d5-bde7-6d45b4d39fb8",
  "saleItems": [
    {
      "productId": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
      "productItemQuantity": 12
    }
  ]
  }'
  ```
- Response: 
  ```json
  {
  "data": {
    "userId": "40515b29-e112-4457-9a69-c170b833986d",
    "branchSaleId": "035f4be0-ddec-42d5-bde7-6d45b4d39fb8",
    "totalSalePrice": 96,
    "saleItems": [
      {
        "productId": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
        "productItemQuantity": 12,
        "unitProductItemPrice": 10,
        "discountAmount": 24,
        "totalSaleItemPrice": 96,
        "totalWithoutDiscount": 120,
        "saleItemStatus": "Created",
        "id": "00000000-0000-0000-0000-000000000000"
      }
    ]
  },
  "success": true,
  "message": "Cart created successfully",
  "errors": []
  }
  ```


<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./products-api.md">Previous: Products API</a>
  <a href="./sale-api.md">Next: Sales API</a>
</div>