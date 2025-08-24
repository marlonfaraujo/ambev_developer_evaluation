namespace Ambev.DeveloperEvaluation.ORM.Queries
{
    public class ListSalesQuery
    {
        public const string SELECT = @"
            SELECT
                s.""Id"" AS SaleId,
                s.""SaleNumber"",
                s.""SaleDate"",
                s.""TotalSalePrice"",
                s.""SaleStatus"",
                u.""Id"" AS UserId,
                u.""Username"" AS UserName,
                b.""Id"" AS BranchId,
                b.""Name"" AS BranchName,
                b.""Description"" AS BranchDescription,
                si.""Id"" AS SaleItemId,
                si.""ProductItemQuantity"",
                si.""UnitProductItemPrice"",
                si.""DiscountAmount"",
                si.""TotalSaleItemPrice"",
                si.""TotalWithoutDiscount"",
                si.""SaleItemStatus"",
                p.""Id"" AS ProductId,
                p.""Name"" AS ProductName,
                p.""Description"" AS ProductDescription,
                p.""Price"" AS ProductPrice
            FROM public.""Sales"" s
                INNER JOIN public.""Users"" u ON s.""UserId"" = u.""Id""
                INNER JOIN public.""Branchs"" b ON s.""BranchSaleId"" = b.""Id""
                INNER JOIN public.""SaleItems"" si ON si.""SaleId"" = s.""Id""
                INNER JOIN public.""Products"" p ON si.""ProductId"" = p.""Id""
            WHERE s.""UserId"" = u.""Id""
        ";
    }
}
