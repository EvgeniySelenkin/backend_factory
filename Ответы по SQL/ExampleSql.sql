SELECT * FROM Tank;
SELECT COUNT(*) FROM Tank WHERE Tank.Volume>500;
SELECT Unit.Name, Factory.Name FROM Unit INNER JOIN Factory ON Unit.FactoryId=Factory.Id;
SELECT Unit.Name AS UnitName, Factory.Name AS FactoryName, Factory.Description AS FactoryDescription, COUNT(Tank.UnitId) AS TanksCount, SUM(Tank.Volume) AS TanksVolume, SUM(Tank.MaxVolume) AS TanksMaxVolume FROM ((Unit INNER JOIN Factory ON Unit.FactoryId=Factory.Id) INNER JOIN Tank ON Tank.UnitId=Unit.Id) GROUP BY Unit.Name, Factory.Name, Factory.Description;
SELECT Factory.Name, Sum(Tank.Volume) AS TanksVolume, SUM(Tank.MaxVolume) AS TanksMaxVolume FROM ((Factory INNER JOIN Unit ON Factory.Id=Unit.FactoryId) INNER JOIN Tank ON Unit.Id=Tank.UnitId) GROUP BY Factory.Name;
SELECT Unit.Name FROM Unit INNER JOIN Tank ON Tank.UnitId=Unit.Id WHERE Tank.Volume>1000 GROUP BY Unit.Name;
SELECT Tank.Name FROM Tank INNER JOIN Unit ON Tank.UnitId=Unit.Id WHERE Unit.Name LIKE'Ã%';