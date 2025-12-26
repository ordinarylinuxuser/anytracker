# ⏳ Any Tracker

![.NET MAUI](https://img.shields.io/badge/.NET_MAUI-512BD4?style=for-the-badge&logo=dotnet&logoColor=white) ![Platform](https://img.shields.io/badge/Platform-Android%20|%20iOS%20|%20macOS%20|%20Windows-blue?style=for-the-badge) ![Status](https://img.shields.io/badge/Status-Active-success?style=for-the-badge)

**Any Tracker** is a highly configurable tracker app built with **.NET MAUI**. It empowers users to track any time-based process—from intermittent fasting and Pomodoro timers to pregnancy milestones—simply by switching configurations.

Trackers are defined entirely by JSON configuration files, making the app infinitely extensible.

---

## ✨ Key Features

* **📄 Flexible Configuration:** Define trackers easily via JSON files located in `AnyTracker/Resources/Raw`.
* **🔄 Multi-Stage Tracking:** Support for multiple stages per tracker, complete with custom titles, descriptions, icons, and colors.
* **⏱️ Real-time Control:** Intuitive Start / Stop functionality with live elapsed time display and a visual stage roadmap.
* **🎛️ Instant Switching:** Simple selector UI to toggle active trackers via `tracker_manifest.json`.
* **🔔 Smart Notifications:** Integration hooks via `INotificationService` to keep users updated.

---

## 🚀 Quick Start

### 🧰 Requirements
1.  **.NET SDK:** Compatible with the MAUI version used in this repo.
2.  **IDE:** JetBrains Rider 2025.3.1 (Recommended).
3.  **Toolchains:** Platform-specific dependencies (Android/iOS/macOS/Windows) if targeting those platforms.

### 🏃 Open and Run
1.  **Open:** Load the solution in `JetBrains Rider` (or your preferred IDE).
2.  **Restore:** Run `dotnet restore` in the repository root.
3.  **Build:** Run `dotnet build`.
4.  **Launch:** Run from Rider using the MAUI run target, or use the CLI platform target appropriate for your environment.

---

## 📂 Project Layout

| Directory | Description |
| :--- | :--- |
| **`AnyTracker`** | The core MAUI app project |
| **`AnyTracker/Pages`** | UI pages (e.g., `MainPage.xaml`, `SettingsPage.xaml`) |
| **`AnyTracker/ViewModels`** | View models, including the core `TrackingViewModel.cs` |
| **`AnyTracker/Resources/Raw`** | Storage for tracker JSON files and `tracker_manifest.json` |

---

## ⚙️ Configuration & Customization

The heart of Any Tracker is in the JSON. Active tracker definitions are stored in `AnyTracker/Resources/Raw`.

### The Manifest (`tracker_manifest.json`)
This file lists all available trackers. Example entry:
```json
{
  "Name": "Intermittent Fasting",
  "FileName": "config_fasting.json",
  "Icon": "🍽️"
}

```

### 🛠️ How to Add a New Tracker

1. **Create:** Add a new JSON config file in `Resources/Raw` (follow the structure of `config_fasting.json`).
2. **Register:** Add an entry to `tracker_manifest.json` including the `Name`, `FileName`, and `Icon`.
3. **Launch:** The Settings page selector will automatically pick it up, and `TrackingViewModel` will load it via `LoadTrackerConfig`.

---

## 🧠 Under the Hood

1. **Data Binding:** The UI binds directly to `TrackingViewModel` properties like `TrackerTitle`, `Stages`, `CurrentStage`, and `ElapsedTime`.
2. **Initialization:** On startup (or setting change), the ViewModel loads the active JSON configuration.
3. **State Management:** Start/Stop operations update the timer and trigger notifications via `INotificationService`.
4. **Settings Flow:**
* `SettingsPage` opens `TrackerSelectorPage`.
* Upon selection, an action invokes `LoadTrackerConfig` with the new file name.



---

## 🗺️ Roadmap

* [ ] 🐧 **Platform Support:** Improve packaging and add platform-specific run instructions. Currently only on android but in future add support for windows, mac and iOS. 
* [ ] 💾 **Persistence:** Save the last active tracker and tracking state across app restarts.
* [ ] 🌍 **Localization:** Add support for translating tracker definitions and UI.
* [ ] ✏️ **In-App Editor:** UI for creating and editing tracker JSON files directly in the app.
* [ ] 📲 **Advanced Features:** Background tracking integration and advanced notifications.
* [ ] 🧪 **Testing:** Add unit tests for `TrackingViewModel` and config parsing.
* [ ] 📦 **Ecosystem:** Provide a sample tracker pack and import/export flow.

---

## 🤝 Contributing

Contributions are welcome!

1. Fork the repository and create a feature branch.
2. Add tests for any new functionality.
3. Open a pull request with a clear description of your changes.

---

### 📚 Useful Files Reference

* `AnyTracker/ViewModels/TrackingViewModel.cs` (Core logic)
* `AnyTracker/Resources/Raw/config_fasting.json` (Sample config)
* `AnyTracker/Pages/SettingsPage.xaml` (Selection UI)
* `AnyTracker/MainPage.xaml` (Main UI)
