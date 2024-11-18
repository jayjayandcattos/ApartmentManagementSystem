Public Class Form2

    ' Declare a variable to hold the DashboardForm instance
    Private dashboardForm As DashboardForm

    ' This method loads the child forms into Panel1
    Sub childform(ByVal panel As Form)
        Panel1.Controls.Clear()
        panel.TopLevel = False
        panel.FormBorderStyle = FormBorderStyle.None
        panel.Dock = DockStyle.Fill
        Panel1.Controls.Add(panel)
        panel.Show()
    End Sub

    ' Button1_Click event handler to show the DashboardForm inside Panel1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Check if the dashboardForm is already created, if not create it
        If dashboardForm Is Nothing Then
            dashboardForm = New DashboardForm()
        End If

        ' Load DashboardForm into Panel1 using the childform method
        childform(dashboardForm)
    End Sub

    ' Button2_Click event handler to show the ApartmentsForm and pass the DashboardForm reference
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Create a new instance of ApartmentsForm
        Dim apartmentsForm As New ApartmentsForm()

        ' Pass the reference of DashboardForm to ApartmentsForm
        apartmentsForm.DashboardFormInstance = dashboardForm

        ' Load ApartmentsForm into Panel1 using the childform method
        childform(apartmentsForm)
    End Sub

    ' Button3_Click event handler to show the TenantsForm and pass the DashboardForm reference
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Create a new instance of TenantsForm
        Dim tenantsForm As New TenantsForm()

        ' Pass the reference of DashboardForm to TenantsForm
        tenantsForm.DashboardFormInstance = dashboardForm

        ' Load TenantsForm into Panel1 using the childform method
        childform(tenantsForm)
    End Sub

    ' Button4_Click event handler to show the PaymentsForm (assuming PaymentsForm is already created)
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Assuming PaymentsForm is a predefined form, create an instance of it
        Dim paymentsForm As New PaymentsForm()

        ' Load PaymentsForm into Panel1 using the childform method
        childform(paymentsForm)
    End Sub

    ' To track if the button is clicked for closing the form properly
    Private buttonClicked As Boolean = False

    ' Button5_Click event handler to close the Form2
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        buttonClicked = True

        ' Show Form1 (main form) and close Form2
        Dim mainForm As New Form1()
        mainForm.Show()
        Me.Close()
    End Sub

    ' Form2_FormClosed event handler to ensure the application exits only if needed
    Private Sub Form2_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        If Not buttonClicked Then
            Application.Exit() ' Close the application if Form2 is closed without clicking Button5
        End If
    End Sub

    ' Form2_Load event handler to initialize Form2
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' If you want to load the DashboardForm immediately on Form2 load
        If dashboardForm Is Nothing Then
            dashboardForm = New DashboardForm()
        End If
        childform(dashboardForm)
    End Sub
End Class
