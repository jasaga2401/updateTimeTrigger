Imports MySql.Data.MySqlClient

Public Class Form1

    Dim currentIndex As Integer = 0
    Dim customerList As New DataTable()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadCustomerData()
        DisplayCustomer(currentIndex)

    End Sub

    Private Sub LoadCustomerData()

        Dim connectionString As String = "Server=localhost;User ID=root;password=12Yellow34!;database=dbtrigger"

        Using conn As New MySqlConnection(connectionString)

            conn.Open()

            Using cmd As New MySqlCommand("sp_get_all_customers", conn)
                cmd.CommandType = CommandType.StoredProcedure

                Dim adapter As New MySqlDataAdapter(cmd)
                adapter.Fill(customerList)

            End Using

        End Using
    End Sub

    Private Sub DisplayCustomer(index As Integer)
        If customerList.Rows.Count > 0 AndAlso index >= 0 AndAlso index < customerList.Rows.Count Then
            txtCid.Text = customerList.Rows(index)("cid").ToString()
            txtTitle.Text = customerList.Rows(index)("title").ToString()
            txtForename.Text = customerList.Rows(index)("forename").ToString()
            txtSurname.Text = customerList.Rows(index)("surname").ToString()
            txtUsername.Text = customerList.Rows(index)("username").ToString()
            txtPassword.Text = customerList.Rows(index)("passwd").ToString()
            txtLastModified.Text = customerList.Rows(index)("lastModified").ToString()
        End If
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If currentIndex < customerList.Rows.Count - 1 Then
            currentIndex += 1
            DisplayCustomer(currentIndex)
        Else
            MessageBox.Show("You are at the last record.")
        End If
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If currentIndex > 0 Then
            currentIndex -= 1
            DisplayCustomer(currentIndex)
        Else
            MessageBox.Show("You are at the first record.")
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        Dim connectionString As String = "Server=localhost;User ID=root;password=12Yellow34!;database=dbtrigger"
        Using conn As New MySqlConnection(connectionString)
            Try
                conn.Open()

                Using cmd As New MySqlCommand("sp_updateCustomer", conn)
                    ' Add parameters with values from the textboxes
                    cmd.Parameters.AddWithValue("@cid", txtCid.Text)
                    cmd.Parameters.AddWithValue("@title", txtTitle.Text)
                    cmd.Parameters.AddWithValue("@forename", txtForename.Text)
                    cmd.Parameters.AddWithValue("@surname", txtSurname.Text)
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text)
                    cmd.Parameters.AddWithValue("@passwd", txtPassword.Text)

                    ' Execute the update command
                    cmd.ExecuteNonQuery()

                    ' Refresh data in the DataTable to reflect updated information
                    customerList.Clear()
                    LoadCustomerData()
                    DisplayCustomer(currentIndex)

                    MessageBox.Show("Record updated successfully.")
                End Using

            Catch ex As MySqlException
                MessageBox.Show("Error updating record: " & ex.Message)
            End Try
        End Using

    End Sub
End Class
