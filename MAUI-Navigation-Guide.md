# MAUI Navigation & Pages — Step-by-Step Guide

A practical, concept-first guide for navigating between pages in a .NET MAUI app
using **Shell Navigation** — the standard MAUI approach.

---

## Table of Contents

1. [What is a Page?](#1-what-is-a-page)
2. [What is Shell?](#2-what-is-shell)
3. [Setting Up a TabBar (Bottom Tabs)](#3-setting-up-a-tabbar-bottom-tabs)
4. [Creating a New Page](#4-creating-a-new-page)
5. [Registering Routes](#5-registering-routes)
6. [Navigating Between Tabs](#6-navigating-between-tabs)
7. [Pushing a Detail Page](#7-pushing-a-detail-page)
8. [Passing Data to a Page](#8-passing-data-to-a-page)
9. [Receiving Data on the Destination Page](#9-receiving-data-on-the-destination-page)
10. [Going Back](#10-going-back)
11. [Displaying a List with CollectionView](#11-displaying-a-list-with-collectionview)
12. [Navigation Cheat Sheet](#12-navigation-cheat-sheet)

---

## 1. What is a Page?

In MAUI, every screen is a **page** — a class that extends `ContentPage`. A page
has two files:

```
MyPage.xaml        ← layout (what it looks like)
MyPage.xaml.cs     ← code-behind (what it does)
```

`MyPage.xaml.cs` is always declared as `partial` because the XAML compiler
generates the other half automatically (including `InitializeComponent()` and
all `x:Name` fields).

```csharp
// MyPage.xaml.cs
public partial class MyPage : ContentPage
{
    public MyPage()
    {
        InitializeComponent(); // generated — wires up the XAML
    }
}
```

---

## 2. What is Shell?

`AppShell.xaml` is the **navigation host** for the entire app. It defines the
structure — tabs, flyout menus, the route map — and is set as the root in
`App.xaml.cs`:

```csharp
MainPage = new AppShell();
```

Shell uses **URL-like routes** to navigate. Every page in your app has an
address, just like a web page.

---

## 3. Setting Up a TabBar (Bottom Tabs)

Wrap your pages in a `TabBar` inside `AppShell.xaml`. On mobile this renders as
bottom tabs; on Windows/macOS as top tabs.

```xml
<!-- AppShell.xaml -->
<Shell ...
    xmlns:pages="clr-namespace:MyApp.Pages">

    <TabBar>

        <Tab Title="Home" Icon="home.png">
            <ShellContent Route="home"
                          ContentTemplate="{DataTemplate local:MainPage}" />
        </Tab>

        <Tab Title="Customers" Icon="customers.png">
            <ShellContent Route="customers"
                          ContentTemplate="{DataTemplate pages:CustomersPage}" />
        </Tab>

    </TabBar>

</Shell>
```

> **`ContentTemplate` vs `Content`** Use `ContentTemplate` (with `DataTemplate`)
> so the page is created **lazily** — only when the tab is first opened, not at
> app startup.

---

## 4. Creating a New Page

**XAML file** — declare the layout and give elements names with `x:Name`:

```xml
<!-- Pages/CustomersPage.xaml -->
<ContentPage xmlns="..."
             x:Class="MyApp.Pages.CustomersPage"
             Title="Customers">

    <Label x:Name="WelcomeLabel" Text="Hello" />

</ContentPage>
```

**Code-behind** — access named elements by their `x:Name`:

```csharp
// Pages/CustomersPage.xaml.cs
namespace MyApp.Pages;

public partial class CustomersPage : ContentPage
{
    public CustomersPage()
    {
        InitializeComponent();
        WelcomeLabel.Text = "Welcome!"; // x:Name field, available after InitializeComponent()
    }
}
```

> **Why does `WelcomeLabel` exist in code?** `x:Name` in XAML generates a
> private field in a companion partial class
> (`obj/.../CustomersPage.xaml.g.cs`). Your code-behind is the other half of the
> same partial class. They're merged at compile time.

---

## 5. Registering Routes

Tab pages declared in `AppShell.xaml` are registered automatically. Any **other
page** you want to navigate to (e.g. a detail page) must be registered manually
in `AppShell.xaml.cs`:

```csharp
// AppShell.xaml.cs
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("customer-detail", typeof(Pages.CustomerDetailPage));
        Routing.RegisterRoute("ticket-detail",   typeof(Pages.TicketDetailPage));
    }
}
```

The string `"customer-detail"` is the route name — you'll use it in
`GoToAsync()`.

---

## 6. Navigating Between Tabs

To **switch tabs** from code, use an **absolute route** prefixed with `//`:

```csharp
await Shell.Current.GoToAsync("//customers");
```

The `//` means "from the root of the shell" — it jumps directly to that tab
regardless of where you currently are.

```csharp
// From a button click on the Home page
private async void OnGoToCustomersClicked(object? sender, EventArgs e)
{
    await Shell.Current.GoToAsync("//customers");
}
```

---

## 7. Pushing a Detail Page

To **push a new page on top** of the current tab (like opening a detail screen),
use a **relative route** — no `//` prefix:

```csharp
await Shell.Current.GoToAsync("customer-detail");
```

This pushes `CustomerDetailPage` onto the navigation stack. The user can press
the back button (or swipe back on iOS) to return.

```
Tab: Customers
  └── CustomersPage          ← current
        └── CustomerDetailPage  ← pushed on top
```

---

## 8. Passing Data to a Page

MAUI Shell passes data using a **query string** appended to the route:

```csharp
await Shell.Current.GoToAsync($"customer-detail?CustomerId={customer.Id}");
```

This is the only way to pass data via the URL. Path parameters (`:id`) do not
exist in MAUI Shell — only query strings (`?key=value`).

You can pass multiple values:

```csharp
await Shell.Current.GoToAsync($"customer-detail?CustomerId={id}&Mode=edit");
```

---

## 9. Receiving Data on the Destination Page

On the destination page, use `[QueryProperty]` to map a URL query key to a C#
property:

```csharp
// CustomerDetailPage.xaml.cs

[QueryProperty(nameof(CustomerId), "CustomerId")]
//                    ↑ C# property    ↑ URL key name (?CustomerId=...)
public partial class CustomerDetailPage : ContentPage
{
    public int CustomerId
    {
        set => LoadCustomer(value); // setter fires automatically before page appears
    }

    private void LoadCustomer(int id)
    {
        // fetch and display the customer with this id
    }
}
```

> MAUI calls the **setter** automatically with the parsed value before the page
> is shown. For complex objects, pass an ID and look up the object — don't try
> to pass whole objects through query strings.

---

## 10. Going Back

**Hardware/gesture back** (Android back button, iOS swipe) works automatically
with Shell — no code needed.

**Programmatic back** — navigate up one level:

```csharp
await Shell.Current.GoToAsync("..");
```

**Back to a specific tab root** — use absolute route:

```csharp
await Shell.Current.GoToAsync("//customers"); // pops the stack and switches to Customers tab
```

---

## 11. Displaying a List with CollectionView

`CollectionView` is the recommended MAUI control for lists. Use
`SelectionChanged` to handle taps and navigate to a detail page.

**XAML:**

```xml
<CollectionView x:Name="CustomersList"
                SelectionMode="Single"
                SelectionChanged="OnCustomerSelected">
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="models:Customer">
            <Label Text="{Binding Name}" />
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

> `x:DataType` enables **compiled bindings** — faster, and gives IntelliSense on
> `{Binding}` expressions.

**Code-behind:**

```csharp
public CustomersPage()
{
    InitializeComponent();
    CustomersList.ItemsSource = myListOfCustomers;
}

private async void OnCustomerSelected(object? sender, SelectionChangedEventArgs e)
{
    if (e.CurrentSelection.FirstOrDefault() is not Customer selected) return;

    // Reset so the same row can be tapped again after returning
    if (sender is CollectionView list) list.SelectedItem = null;

    await Shell.Current.GoToAsync($"customer-detail?CustomerId={selected.Id}");
}
```

---

## 12. Navigation Cheat Sheet

### Route syntax

| URL                            | Effect                                            |
| ------------------------------ | ------------------------------------------------- |
| `//customers`                  | Switch to the Customers tab (absolute, from root) |
| `customer-detail`              | Push CustomerDetailPage onto current tab's stack  |
| `customer-detail?CustomerId=1` | Push and pass data                                |
| `..`                           | Go back one page                                  |

### `//` vs no prefix

| Prefix            | Type     | Use when                                        |
| ----------------- | -------- | ----------------------------------------------- |
| `//home`          | Absolute | Jumping between tabs or resetting to a tab root |
| `customer-detail` | Relative | Pushing a sub-page on top of the current screen |

### Route registration

| Page type                   | How it's registered                                                   |
| --------------------------- | --------------------------------------------------------------------- |
| Tab page in `AppShell.xaml` | **Automatic**                                                         |
| Detail / sub-page           | `Routing.RegisterRoute("name", typeof(MyPage))` in `AppShell.xaml.cs` |

### Passing & receiving data

```csharp
// Sender — append to route URL
await Shell.Current.GoToAsync($"detail-page?ItemId={id}");

// Receiver — attribute on the page class
[QueryProperty(nameof(ItemId), "ItemId")]
public partial class DetailPage : ContentPage
{
    public int ItemId { set => LoadItem(value); }
}
```

### Why IntelliSense shows `x:Name` errors but the build succeeds

`x:Name` fields are generated into `obj/.../MyPage.xaml.g.cs` at **build time**.
The language server sometimes hasn't read that file yet. Build the project, then
run **"Restart Language Server"** from the Command Palette — the red squiggles
will disappear.

---

_Built with .NET MAUI — Shell Navigation. Covers TabBar, detail pages, query
params, CollectionView._
