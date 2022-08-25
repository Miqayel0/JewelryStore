CREATE OR ALTER PROCEDURE dbo.[sp_getEmployeeToAssign]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EmployeeId INTEGER = NULL
		   ,@EmployeeIsAssigned BIT = 0
		   ,@Date DATETIME = GETUTCDATE();

	SELECT TOP (1) @EmployeeId = e.[Id]
	FROM dbo.[Employee] e
	WHERE e.[Id] NOT IN (
		SELECT DISTINCT o.[EmployeeId]
		FROM dbo.[Order] o
		WHERE o.[Status] IN (1, 2)
	)

	IF (@EmployeeId IS NULL)
	BEGIN
	SELECT
		@EmployeeId = d.[EmployeeId],
		@EmployeeIsAssigned = 1,
		@Date = d.[FinishDate]
	FROM (
		SELECT TOP (1) o.[EmployeeId]
				,MAX(DATEADD(SECOND, oi.[ExpectedTime], o.[StartDate])) AS [FinishDate]
		FROM dbo.[Order] o
		INNER JOIN (
			SELECT oi.[OrderId]
					,SUM(oi.[ExpectedTime]) AS [ExpectedTime]
			FROM dbo.[OrderItem] oi
			GROUP BY oi.[OrderId]
		) oi ON o.[Id] = oi.[OrderId]
		WHERE [Status] IN (1, 2)
		GROUP BY o.[EmployeeId]
		ORDER BY [FinishDate]
	) d
	END

	SELECT @EmployeeId AS [Id]
		  ,@Date AS [FinishDate]
		  ,@EmployeeIsAssigned AS [Assigned]

	RETURN;
END
