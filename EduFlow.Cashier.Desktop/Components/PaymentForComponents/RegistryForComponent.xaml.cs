using EduFlow.Domain.Enums;
using System.Windows.Controls;

namespace EduFlow.Cashier.Desktop.Components.PaymentForComponents;

/// <summary>
/// Interaction logic for RegistryForComponent.xaml
/// </summary>
public partial class RegistryForComponent : UserControl
{
    public long Id { get; set; }
    public RegistryForComponent()
    {
        InitializeComponent();
    }

    public void SetValues(long id, int number, double amount, string? description, DateTime date, PaymentType type)
    {
        this.Id = id;
        tbNumber.Text = number.ToString();
        tbPrice.Text = amount.ToString();
        tbDescription.Text = description ?? "...";
        TbDate.Text = date.ToString("dd:MM:yyyy");
        tbType.Text = type switch
        {
            PaymentType.Cash => "Naqt",
            PaymentType.Card => "Plastik",
            PaymentType.Transfer => "O'tqazma",
            PaymentType.Credit => "Nasiya",
            PaymentType.Other => "Boshqa",
            _ => "Aniq emas"
        };
    }
}
