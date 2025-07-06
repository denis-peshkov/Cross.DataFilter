[![Nuget](https://img.shields.io/nuget/v/Cross.DataFilter.svg)](https://nuget.org/packages/Cross.DataFilter/) [![Documentation](https://img.shields.io/badge/docs-wiki-yellow.svg)](https://github.com/denis-peshkov/Cross.DataFilter/wiki)

# Cross.DataFilter

A lightweight .NET library designed to simplify data filtering, sorting, and pagination in Entity Framework Core applications. The library provides a robust set of tools for handling common data access patterns with a focus on performance and extensibility.

Main Features:

- **Advanced Pagination**
    - Efficient query pagination with skip/take operations
    - Support for both IQueryable and IEnumerable collections
    - Built-in page size and count tracking

- **Dynamic Sorting**
    - Attribute-based sorting configuration
    - Multi-level property sorting
    - Case-insensitive string comparison
    - Support for complex object sorting

- **AutoComplete Support**
    - Ready-to-use autocomplete query handlers
    - Built-in pagination for large datasets
    - Configurable ordering
    - Support for grouped results

- **Flexible Architecture**
    - CQRS-friendly design
    - Integration with MediatR
    - Extensible query handlers
    - Support for custom filtering logic

The library is particularly useful for building modern web applications that require efficient data handling with minimal boilerplate code.

## Install with nuget.org:

https://www.nuget.org/packages/Cross.DataFilter

## Installation

Clone repository or Install Nuget Package
```
Install-Package Cross.DataFilter
```

## Quick Start

### Basic Pagination Query

```csharp
public class MyPaginatedQuery : PaginatedItemsQuery<MyFilter, MyEntity>
{
    public MyPaginatedQuery(MyFilter request)
        : base(request) { }
}
```

### AutoComplete Implementation

```csharp
public class MyAutoCompleteQuery : AutoCompleteQuery
{
    public MyAutoCompleteQuery(int? page, int? pageSize, MyFilter filter)
        : base(page, pageSize, filter) { }
}
```

## Configuration

### Basic Setup

```csharp
services.AddCrossDataFilter();
```

## Issues and Pull Request

Contribution is welcomed. If you would like to provide a PR please add some testing.

## How To's

Please use [Wiki](https://github.com/denis-peshkov/Cross.DataFilter/wiki) for documentation and usage examples.

### Complete usage examples can be found in the test project ###
Note - test project is not a part of nuget package. You have to clone repository.

## Roadmap:

### Memory Optimization
- [ ] Large dataset optimization
  - Общая оптимизация для больших наборов данных:
    - Эффективные SQL запросы
    - Правильное использование индексов
    - Оптимизация JOIN операций
    - Эффективные стратегии подсчета общего количества записей
    - Асинхронная загрузка данных
    - Умное кэширование результатов
- [ ] Streaming results for large datasets
  - Специфический подход обработки данных:
    - Потоковая передача данных без загрузки всего набора в память
    - Использование yield return для постепенной обработки
    - Server-Sent Events для real-time передачи
    - Чтение данных частями (batch processing)
- [ ] Parallel query support
  - Параллельное выполнение запросов для повышения производительности:
    - Распараллеливание тяжелых операций фильтрации
    - Concurrent выполнение агрегаций
    - Асинхронная загрузка связанных данных
    - Балансировка нагрузки между потоками
    - Контроль за количеством параллельных запросов
- [ ] Query profiling
  - Анализ и оптимизация производительности запросов:
    - Измерение времени выполнения запросов
    - Отслеживание используемых ресурсов
    - Анализ планов выполнения
    - Выявление узких мест
    - Сбор метрик для разных типов запросов
    - Логирование проблемных паттернов
- [ ] Memory-efficient data structures
  - Фокус на **выборе структур данных**:
    - Использование ArrayPool<T> вместо обычных массивов
    - Применение Span<T>/Memory<T> для работу с памятью без аллокаций
    - Структуры вместо классов где это уместно
    - Использование компактных коллекций (например, BitArray вместо bool[])
- [ ] Memory usage optimization
  - Фокус на **эффективном использовании памяти**:
    - Правильное управление жизненным циклом объектов
    - Своевременное освобождение ресурсов
    - Оптимизация размера кэша
    - Контроль за утечками памяти
    - Мониторинг общего потребления памяти
    - Уменьшение пиковых нагрузок на память
- [ ] Garbage collection optimization
- Фокус на **настройке работы сборщика мусора**:
  - Выбор подходящего режима GC (Server/Workstation)
  - Настройка поколений GC
  - Оптимизация частоты сборок
  - Управление размером Large Object Heap
  - Настройка порогов для разных поколений
  - Минимизация фрагментации кучи

### AutoComplete
- [ ] Customizable search algorithms
- [ ] Performance improvements

### Filtering
- [ ] Dynamic filters
  - Важная и базовая функциональность:
    - Создание фильтров "на лету"
    - Изменение условий фильтрации без перезагрузки
    - UI для интерактивного построения фильтров
    - **Это основа современной системы фильтрации**
- [ ] Composite conditions
  - Необходимо для сложной фильтрации:
    - Комбинирование условий через AND/OR
    - Группировка условий в скобках
    - Пример: (Status = 'Active' AND Price > 100) OR (IsSpecial = true)
    - **Нужно если простых фильтров недостаточно**
- [ ] Custom comparison operators
  - Может быть избыточным:
    - Стандартных операторов (=, >, <, !=, LIKE) часто достаточно
    - Имеет смысл только при специфичных требованиях
    - Пример: геопространственные операторы, специальные текстовые сравнения
    - **Можно отложить или убрать, если нет особых требований**
- [ ] Nested property filters
  - Зависит от структуры данных:
    - Нужно если у вас сложные объекты с вложенностью
    - Пример: User.Address.City или Order.Items[].Product.Category
    - **Необходимо только при работе со сложными объектами**

### Sorting
- [x] Multi-level sorting
  - **Важная базовая функциональность:**
    - Сортировка по нескольким полям одновременно
    - Пример: сначала по статусу, потом по дате, потом по имени
    - Разные направления сортировки для разных полей
    - **Это стандартное требование для современных таблиц**
- [ ] Nested property sorting
  - Зависит от структуры данных:
    - Нужно при работе со сложными объектами
    - Пример: сортировка по User.Profile.LastName
    - Полезно при связанных данных
    - **Необходимо только при наличии вложенных объектов**
- [ ] Custom comparators
  - Может быть избыточным:
    - Стандартных компараторов часто достаточно
    - Нужно для специальных случаев:
      - Сложная логика сравнения
      - Специфичные бизнес-правила
      - Кастомный порядок сортировки
    - **Можно отложить до появления особых требований**
- [ ] Sort expression building
  - Зависит от реализации:
    - Построение динамических выражений сортировки
    - Преобразование UI-команд в выражения для БД
    - Интеграция с ORM или другими системами
    - **Техническая необходимость зависит от архитектуры**

### Database Integration
- [x] EF Core optimization
- [ ] MongoDB support
