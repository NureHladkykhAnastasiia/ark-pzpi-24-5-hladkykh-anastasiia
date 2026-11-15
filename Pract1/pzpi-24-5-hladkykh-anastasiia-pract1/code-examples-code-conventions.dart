// В.1 Структура коду та іменування
enum OrderStatus { pending, processing, completed, cancelled }

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
}

// В.2 Форматування коду (до та після dart format)
double calcBefore(double p,int q){if(q<=0){return 0;}return p*q;}

double calculateOrderAmount(double price, int quantity) {
  if (quantity <= 0) {
    return 0.0;
  }
  return price * quantity;
}

// В.3 Іменування та іменовані константи
double getFinalPriceBad(double a, int b) {
  final x = a * b;
  return x > 1000 ? x * 0.9 : x;
}

const double discountThreshold = 1000.0;
const double discountRate = 0.1;

double calculateDiscountedTotal(double price, int quantity) {
  final total = price * quantity;
  final hasDiscount = total > discountThreshold;
  return hasDiscount ? total * (1 - discountRate) : total;
}

// В.4 Коментарі та документування API
/// Обчислює вартість замовлення з урахуванням округлення до цілих одиниць.
///
/// Приймає базову ціну [unitPrice] та кількість [rawQuantity].
/// Повертає підсумкову суму у гривнях.
double calculateDeliveryBatchCost(double unitPrice, double rawQuantity) {
  // Служба доставки приймає лише цілі упаковки товару.
  final packagesCount = rawQuantity.ceil();
  return unitPrice * packagesCount;
}

// В.5 Безпека типів (Null Safety) та незмінність (final)
String formatCustomerGreetingBad(String? userName) {
  var greeting = 'Вітаємо, ' + userName! + '!';
  return greeting;
}

String formatCustomerGreeting(String? userName) {
  final safeName = userName ?? 'Гість';
  return 'Вітаємо, $safeName!';
}

// В.6 Обробка помилок (Guard Clauses)
void validateItemPrice(double price) {
  if (price <= 0) {
    throw ArgumentError.value(
      price,
      'price',
      'Ціна товару повинна бути більшою за нуль.',
    );
  }
}
