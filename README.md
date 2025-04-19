# Мини ДЗ 2 по КПО
> Выполнил: Гювенч Эмрэ Халитович БПИ238
---

## 1. Добавленные функции

| Требование                                 | Где в коде                                       |
|---------------------------------------------------|------------------------------------------------------------|
| **a. Добавить / удалить животное**                | `AnimalsController`, `Animal`                              |
| **b. Добавить / удалить вольер**                  | `EnclosuresController`, `Enclosure`                        |
| **c. Переместить животное**                       | `AnimalTransferService`, `TransfersController`             |
| **d. Просмотреть расписание кормлений**           | `FeedingSchedulesController`                               |
| **e. Добавить кормление в расписание**            | `FeedingSchedulesController`, `FeedingSchedule`            |
| **f. Статистика зоопарка**                        | `ZooStatisticsService`, `StatisticsController`             |
| **+ Учёт кормов** (что‑бы не кончилась капуста)   | `FeedStock`, `FeedingOrganizationService`                  |
| **+ События о низком остатке корма**              | `FeedStockLowEvent`, лог в `InMemoryEventDispatcher`        |
| **+ Чистота вольеров (уборка)**                   | `Enclosure.Clean()`, проверка в `AddAnimal()`              |
| **+ Лечение животных**                            | `HealthService`, `TreatmentCase`, события *Started/Finished* |

Покрытие тестами xUnit ≈ 75 %.

---

## 2. Какое DDD я применил

| Концепт DDD                        | Пример(ы) в проекте |
|-----------------------------------|---------------------|
| **Entity**                        | `Animal`, `Enclosure`, `FeedStock`, `TreatmentCase` |
| **Value Object**                  | `AnimalId`, `EnclosureId`, `Quantity`               |
| **Domain Event**                  | `AnimalMovedEvent`, `FeedingTimeEvent`, `FeedStockLowEvent`, `TreatmentStartedEvent`, `TreatmentFinishedEvent` |
| **Aggregate Root**                | `Enclosure` агрегирует список `AnimalId`, `FeedStock` агрегирует остаток по типу пищи |
| **Domain Service** (use‑case)     | `AnimalTransferService`, `FeedingOrganizationService`, `HealthService`, `ZooStatisticsService` |
| **Repository (Contract)**         | интерфейсы `IAnimalRepository`, `IFeedStockRepository` в слое *Application* |
| **Anti‑Corruption**               | слой *Infrastructure* (in‑memory реализация + логер событий) |

---

## 3. Как соблюдена Clean Architecture

* **Слои и зависимости**  
*Domain* ничего ни о ком не знает.  
Все внешние зависимости проходят через интерфейсы (репозитории, `IEventDispatcher`).

* **Изоляция бизнес‑правил**  
  Проверки вместимости вольера, совместимости хищник/травоядный, остатка кормов и т.п. живут **внутри** сущностей (`Enclosure`, `FeedStock`) — а не в контроллерах.

* **Open/Closed**  
  Новые фичи (лечение, склад) добавлены отдельными сервисами и сущностями, старый код не правился.

