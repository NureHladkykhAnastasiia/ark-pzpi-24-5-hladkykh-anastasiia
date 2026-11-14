/// Можливі стани обробки замовлення клієнта.
enum OrderStatus { pending, processing, completed, cancelled }

/// Модель замовлення, оформлена за стандартами Effective Dart.
class CustomerOrder {
  final String orderId;
  final String customerName;
  final List<double> _itemPrices;
  OrderStatus _status = OrderStatus.pending;

  CustomerOrder({
    required this.orderId,
    required this.customerName,
    List<double>? initialPrices,
  }) : _itemPrices = initialPrices ?? [];

  OrderStatus get status => _status;

  double get totalAmount =>
      _itemPrices.fold(0.0, (sum, price) => sum + price);

  void addItemPrice(double price) {
    if (price <= 0) {
      throw ArgumentError.value(
        price,
        'price',
        'Ціна товару повинна бути більшою за нуль.',
      );
    }
    _itemPrices.add(price);
  }

  void completeOrder() {
    if (_itemPrices.isEmpty) {
      throw StateError('Неможливо завершити порожнє замовлення.');
    }
    _status = OrderStatus.completed;
  }
}

void main() {
  final order = CustomerOrder(
    orderId: 'ORD-2026-001',
    customerName: 'Гладких Анастасія',
  );

  order.addItemPrice(450.50);
  order.addItemPrice(1200.00);
  order.completeOrder();

  print(
    'Замовлення ${order.orderId} (${order.customerName}): '
    'сума = ${order.totalAmount.toStringAsFixed(2)} грн, '
    'статус = ${order.status.name}',
  );
}
