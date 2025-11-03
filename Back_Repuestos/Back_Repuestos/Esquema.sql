
CREATE TABLE `Categorias` (
    `IdCategoria` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Desctripcion` longtext CHARACTER SET utf8mb4 NULL,
    CONSTRAINT `PK_Categorias` PRIMARY KEY (`IdCategoria`)
) 

CREATE TABLE `Usuarios` (
    `IdUsuario` int NOT NULL AUTO_INCREMENT,
    `Nombre_apellido` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `DNI` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Direccion` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Correo` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `user` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Contrasenia` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `rol` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `CodigoVerificacion` longtext CHARACTER SET utf8mb4 NULL,
    `CodigoExpira` datetime(6) NULL,
    CONSTRAINT `PK_Usuarios` PRIMARY KEY (`IdUsuario`)
);

CREATE TABLE `UsuariosTemporales` (
    `IdTemporal` int NOT NULL AUTO_INCREMENT,
    `Nombre_apellido` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `DNI` varchar(8) CHARACTER SET utf8mb4 NOT NULL,
    `Direccion` longtext CHARACTER SET utf8mb4 NULL,
    `Correo` longtext CHARACTER SET utf8mb4 NOT NULL,
    `User` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Contrasenia` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Rol` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CodigoVerificacion` longtext CHARACTER SET utf8mb4 NOT NULL,
    `FechaExpira` datetime(6) NOT NULL,
    CONSTRAINT `PK_UsuariosTemporales` PRIMARY KEY (`IdTemporal`)
);

CREATE TABLE `Productos` (
    `idProducto` int NOT NULL AUTO_INCREMENT,
    `Nombre` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Descrpicion` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PrecioCompra` decimal(65,30) NOT NULL,
    `PrecioVena` decimal(65,30) NOT NULL,
    `stock` int NOT NULL,
    `Imagen` longblob NULL,
    `IdCategoria` int NOT NULL,
    CONSTRAINT `PK_Productos` PRIMARY KEY (`idProducto`),
    CONSTRAINT `FK_Productos_Categorias_IdCategoria` FOREIGN KEY (`IdCategoria`) REFERENCES `Categorias` (`IdCategoria`) ON DELETE CASCADE
);

CREATE TABLE `Ventas` (
    `IdVenta` int NOT NULL AUTO_INCREMENT,
    `Fecha` datetime(6) NOT NULL,
    `Total` int NOT NULL,
    `IdUsuario` int NOT NULL,
    `Estado` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Ventas` PRIMARY KEY (`IdVenta`),
    CONSTRAINT `FK_Ventas_Usuarios_IdUsuario` FOREIGN KEY (`IdUsuario`) REFERENCES `Usuarios` (`IdUsuario`) ON DELETE CASCADE
);

CREATE TABLE `Resenas` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ProductoId` int NOT NULL,
    `UsuarioId` int NOT NULL,
    `Estrellas` int NOT NULL,
    `Comentario` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Fecha` datetime(6) NOT NULL,
    CONSTRAINT `PK_Resenas` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Resenas_Productos_ProductoId` FOREIGN KEY (`ProductoId`) REFERENCES `Productos` (`idProducto`) ON DELETE CASCADE,
    CONSTRAINT `FK_Resenas_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`IdUsuario`) ON DELETE CASCADE
);

CREATE TABLE `DetalleVentas` (
    `IdDventa` int NOT NULL AUTO_INCREMENT,
    `Cantidad` int NOT NULL,
    `Precio_unidad` int NOT NULL,
    `Subtotal` decimal(65,30) NOT NULL,
    `IdVenta` int NOT NULL,
    `IdProducto` int NOT NULL,
    CONSTRAINT `PK_DetalleVentas` PRIMARY KEY (`IdDventa`),
    CONSTRAINT `FK_DetalleVentas_Productos_IdProducto` FOREIGN KEY (`IdProducto`) REFERENCES `Productos` (`idProducto`) ON DELETE CASCADE,
    CONSTRAINT `FK_DetalleVentas_Ventas_IdVenta` FOREIGN KEY (`IdVenta`) REFERENCES `Ventas` (`IdVenta`) ON DELETE CASCADE
);



