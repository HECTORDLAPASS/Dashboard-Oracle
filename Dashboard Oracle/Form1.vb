Public Class Form1
    Dim query As String
    Dim GenerosDinamicos, nombreedo, nombrenivel As String
    Private ConnectionOracle As New ConnectionOracle()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        nombreedo = "CDMX"
        ListBox1.SelectionMode = SelectionMode.MultiExtended
        'DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        'DataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells

        query = "select * from nivel"
        Dim dataTable As DataTable = ConnectionOracle.SelectQuery(query)
        ListBox1.DataSource = dataTable
        ListBox1.DisplayMember = "nombre"

        query = "select * from estado_nacimiento"
        dataTable = ConnectionOracle.SelectQuery(query)
        ListBox2.DataSource = dataTable
        ListBox2.DisplayMember = "nombre"

    End Sub


    Private Sub graficahym()
        query = GenerosDinamicos
        Dim dtNiveles As DataTable = ConnectionOracle.SelectQuery(query)
        Me.Chart1.Series("hombres").Points.Clear()
        Me.Chart1.Series("Mujeres").Points.Clear()

        For Each row As DataRow In dtNiveles.Rows
            Dim nivel As String = row("nombre").ToString()
            query = "SELECT * FROM VwedadesPromedio where nombre = '" + nivel + "' and edo = '" + nombreedo + "'"
            Dim result As DataTable = ConnectionOracle.SelectQuery(query)

            Me.Chart1.Series("hombres").Points.AddXY(nivel, Convert.ToInt32(result.Rows(0)("hombres")))
            Me.Chart1.Series("Mujeres").Points.AddXY(nivel, Convert.ToInt32(result.Rows(0)("mujeres")))

            Me.Chart1.Series("hombres").Points(Me.Chart1.Series("hombres").Points.Count - 1).Label = Convert.ToInt32(result.Rows(0)("hombres")).ToString()
            Me.Chart1.Series("Mujeres").Points(Me.Chart1.Series("Mujeres").Points.Count - 1).Label = Convert.ToInt32(result.Rows(0)("mujeres")).ToString()
        Next
        graficaedo()
    End Sub
    Private Sub graficaedo()
        Me.Chart2.Series("nivel").Points.Clear()
        query = "SELECT nombre FROM VwedadesPromedio WHERE edo = '" + nombreedo + "'"
        Dim dtedo As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtedo.Rows
            Dim nivel As String = row("nombre").ToString()
            'query = "SELECT nombre, SUM(hombres + mujeres) AS total FROM VwedadesPromedio WHERE edo = 'CDMX' AND nombre = '" + nivel + "' group by nombre;"
            'query = "SELECT nombre, SUM(hombres + mujeres) AS total FROM VwedadesPromedio WHERE edo = 'CDMX' AND nombre = '" + nivel + "' GROUP BY nombre"

            query = "SELECT nombre, SUM(hombres + mujeres) AS total FROM VwedadesPromedio WHERE edo = '" + nombreedo + "' AND nombre = '" + nivel + "' GROUP BY nombre"
            Dim result As DataTable = ConnectionOracle.SelectQuery(query)
            'MsgBox(query)
            Me.Chart2.Series("nivel").Points.AddXY(nivel, Convert.ToInt32(result.Rows(0)("total")))
            Me.Chart2.Series("nivel").Points(Me.Chart2.Series("nivel").Points.Count - 1).Label = Convert.ToInt32(result.Rows(0)("total")).ToString()

        Next
        txtedo.Text = "Distribución de Alumnos por Niveles Estado : " + nombreedo.ToLower.ToString
        graficaedades()
    End Sub
    Private Sub graficaedades()
        Me.Chart3.Series("edades").Points.Clear()
        query = "SELECT nombre FROM VwedadesPromedio WHERE edo = '" + nombreedo + "'"
        Dim dtedo As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtedo.Rows
            Dim nivel As String = row("nombre").ToString()

            query = "SELECT nombre, edo, promedio_edad FROM VwedadesPromedio WHERE edo = '" + nombreedo + "' AND nombre = '" + nivel + "'"

            Dim result As DataTable = ConnectionOracle.SelectQuery(query)

            Me.Chart3.Series("edades").Points.AddXY(nivel, Convert.ToInt32(result.Rows(0)("promedio_edad")))
            Me.Chart3.Series("edades").Points(Me.Chart3.Series("edades").Points.Count - 1).Label = Convert.ToInt32(result.Rows(0)("promedio_edad")).ToString()

        Next
        graficacategorias()
    End Sub
    Private Sub graficacategorias()
        Me.Chart4.Series("categorias").Points.Clear()
        query = "SELECT categoria, SUM(hombres + mujeres) AS total_alumnos FROM Vwcategorias WHERE nombre = '" + nombrenivel + "' AND edo = '" + nombreedo + "' GROUP BY categoria"
        Dim dtcate As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtcate.Rows
            Dim categoria As String = row("categoria").ToString()
            Dim totalAlumnos As Integer = Convert.ToInt32(row("total_alumnos"))

            Me.Chart4.Series("categorias").Points.AddXY(categoria, totalAlumnos)
            Me.Chart4.Series("categorias").Points(Me.Chart4.Series("categorias").Points.Count - 1).Label = totalAlumnos.ToString()
        Next
        txtcategoria.Text = "Distribución de Alumnos por Categoría : " + (nombrenivel.ToLower).ToString()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        dinamico()
    End Sub

    Private Sub dinamico()
        ' Mostrar los elementos seleccionados en un MessageBox
        If ListBox1.SelectedItems.Count = 0 Then
            ListBox1.SetSelected(0, True)
        End If
        GenerosDinamicos = "SELECT * FROM VwedadesPromedio where nombre in ("

        For Each selectedItem As DataRowView In ListBox1.SelectedItems
            GenerosDinamicos += "'" + selectedItem("Nombre").ToString() & "' ,"
            nombrenivel = selectedItem("Nombre").ToString()
        Next

        GenerosDinamicos = GenerosDinamicos.Substring(0, GenerosDinamicos.Length - 1)
        GenerosDinamicos = GenerosDinamicos + ")"

        For Each selectedItem As DataRowView In ListBox2.SelectedItems
            nombreedo = selectedItem("Nombre").ToString()
        Next

        GenerosDinamicos = GenerosDinamicos + "and edo =  '" + nombreedo + "'"
        graficahym()
    End Sub



    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        If ListBox2.SelectedItems.Count = 0 Then
            ListBox2.SetSelected(0, True)
        End If
        dinamico()

    End Sub
End Class
