[Back to README](../README.md)

### Sales

#### GET /sales
```
  bash
curl -X 'GET' \
  'https://localhost:8081/api/Sales?PageNumber=1&PageSize=10' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e'
```
- Response: 
  ```json
  {
    "data": [
      {
        "saleId": "f5ba2389-a493-4f1b-8398-3856e49f2b07",
        "saleNumber": 1,
        "saleDate": "2025-05-23T17:37:31.025405",
        "totalSalePrice": 96,
        "saleStatus": "Created",
        "userId": "40515b29-e112-4457-9a69-c170b833986d",
        "userName": "teste",
        "branchId": "035f4be0-ddec-42d5-bde7-6d45b4d39fb8",
        "branchName": "teste",
        "branchDescription": "teste",
        "saleItemId": "a7b5733b-ec5c-4c41-828a-a8859958ee0c",
        "productItemQuantity": 12,
        "unitProductItemPrice": 10,
        "discountAmount": 24,
        "totalSaleItemPrice": 96,
        "totalWithoutDiscount": 120,
        "saleItemStatus": "Created",
        "productId": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
        "productName": "teste",
        "productDescription": "teste",
        "productPrice": 10,
        "saleItems": [
          {
            "productId": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
            "productItemQuantity": 12,
            "unitProductItemPrice": 10,
            "discountAmount": 24,
            "totalSaleItemPrice": 96,
            "totalWithoutDiscount": 120,
            "saleItemStatus": "Created",
            "id": "a7b5733b-ec5c-4c41-828a-a8859958ee0c"
          }
        ]
      }
    ],
    "success": true,
    "message": "sale retrieved successfully",
    "errors": []
  }
  ```

#### POST /sales
- Description: Add a new sale
- Request Body:
 ```
  bash
 curl -X 'POST' \
  'https://localhost:8081/api/Sales' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e' \
  -d ''
```
- Response: 
  ```json
  {
    "data": {
      "id": "f5ba2389-a493-4f1b-8398-3856e49f2b07",
      "saleNumber": 1,
      "totalSalePrice": 96,
      "saleStatus": "Created",
      "saleItems": [
        {
          "productId": "2da3bb88-6767-4fe7-b0a3-5def8f7985ba",
          "productItemQuantity": 12,
          "unitProductItemPrice": 10,
          "discountAmount": 24,
          "totalSaleItemPrice": 96,
          "totalWithoutDiscount": 120,
          "saleItemStatus": "Created",
          "id": "a7b5733b-ec5c-4c41-828a-a8859958ee0c"
        }
      ]
    },
    "success": true,
    "message": "Sale created successfully",
    "errors": []
  }
  ```

<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./carts-api.md">Previous: Cart API</a>
  <a href="./branch-api.md">Next: Branch API</a>
</div>