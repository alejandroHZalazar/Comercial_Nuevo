CREATE TABLE IF NOT EXISTS promociones (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fk_producto INT NOT NULL,
    activa TINYINT(1) NOT NULL DEFAULT 1,
    fechaDesde DATE NULL,
    fechaHasta DATE NULL,
    FOREIGN KEY (fk_producto) REFERENCES productos(id)
);

CREATE TABLE IF NOT EXISTS promociones_slots (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fk_promocion INT NOT NULL,
    numero INT NOT NULL,
    cantidadRequerida DECIMAL(18,4) NOT NULL,
    descripcion VARCHAR(100) NULL,
    FOREIGN KEY (fk_promocion) REFERENCES promociones(id)
);

CREATE TABLE IF NOT EXISTS promociones_slot_productos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fk_slot INT NOT NULL,
    fk_producto INT NOT NULL,
    FOREIGN KEY (fk_slot) REFERENCES promociones_slots(id),
    FOREIGN KEY (fk_producto) REFERENCES productos(id)
);

CREATE TABLE IF NOT EXISTS ventas_promo_componentes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    fk_ventaDetalle BIGINT NOT NULL,
    fk_slot INT NOT NULL,
    fk_producto INT NOT NULL,
    cantidad DECIMAL(18,4) NOT NULL
);
