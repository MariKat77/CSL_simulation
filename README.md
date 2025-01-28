# CSL_simulation

## Symulacja Systemu Kolejkowego

Projekt realizuje symulację systemu kolejkowego z użyciem biblioteki **CSL** w języku C#. Program analizuje m.in. czas obsługi zgłoszeń, długość kolejki oraz liczbę przejść zgłoszeń przez system.

## 📋 Funkcjonalności

- Generowanie zgłoszeń z losowymi odstępami czasowymi.
- Obsługa zgłoszeń przy użyciu rozkładu wykładniczego.
- Śledzenie statystyk:
  - ⏱️ Średni czas przebywania zgłoszeń w systemie.
  - 📊 Średnia długość kolejki.
  - 📈 Histogram liczby przejść zgłoszeń przez system.
- Wyświetlanie wyników w konsoli.

## 🛠️ Wymagania

- **.NET Framework** (wersja 4.7.2).
- Biblioteka **CSL**:
  - Dodaj ją jako referencję w projekcie.
- Biblioteka MathNet.Iridium-2008.8.16.470

## 🚀 Instalacja

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/MariKat77/CSL_simulation.git
   cd CSL_simulation
   ```
