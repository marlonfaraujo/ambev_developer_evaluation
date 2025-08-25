[Back to README](../README.md)

### Branchs

#### GET /branchs
```
  bash
curl -X 'GET' \
  'https://localhost:8081/api/Branchs?Name=teste&PageNumber=1&PageSize=10' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer e'
```

 Response: 
  ```json
{
  "data": {
    "data": [
      {
        "id": "035f4be0-ddec-42d5-bde7-6d45b4d39fb8",
        "name": "teste",
        "description": "teste"
      }
    ],
    "success": true,
    "message": "Branchs retrieved successfully",
    "errors": []
  },
  "success": true,
  "message": "",
  "errors": []
}
  ```



#### POST /branchs
- Description: Add a new branch
- Request Body:
  ```bash
  curl -X 'POST' \
  'https://localhost:8081/api/Branchs' \
  -H 'accept: text/plain' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI0MDUxNWIyOS1lMTEyLTQ0NTctOWE2OS1jMTcwYjgzMzk4NmQiLCJ1bmlxdWVfbmFtZSI6InRlc3RlIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzQ4MDIwNDUxLCJleHAiOjE3NDgwNDkyNTEsImlhdCI6MTc0ODAyMDQ1MX0.qedNPAnqXLWGOr3QvN_udW51I5i-rGQI4OeTOxdvbQ0' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "teste",
  "description": "teste"
    }'
  ```
- Response: 
  ```json
  {
  "data": {
    "id": "035f4be0-ddec-42d5-bde7-6d45b4d39fb8",
    "name": "teste",
    "description": "teste"
  },
  "success": true,
  "message": "Branch created successfully",
  "errors": []
    }
  ```

<br>
<div style="display: flex; justify-content: space-between;">
  <a href="./carts-api.md">Previous: Carts API</a>
  <a href="./sale-api.md">Next: Sale API</a>
</div>