Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Form2
    Dim query, anio, Grado, idunidad As String
    Dim belen As Integer
    Private ConnectionOracle As New ConnectionOracle()
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.AutoScrollMinSize = New Size(0, 1000)
        Panel1.AutoScroll = True


        query = "SELECT MIN(anio) FROM AlumnosNormalizados"
        anio = Convert.ToInt32(ConnectionOracle.ExecuteScalar(query))

        query = "SELECT MIN(idunidad) FROM AlumnosNormalizados"
        idunidad = Convert.ToInt32(ConnectionOracle.ExecuteScalar(query))

        query = "SELECT MIN(grado) FROM AlumnosNormalizados"
        Grado = Convert.ToInt32(ConnectionOracle.ExecuteScalar(query))
        'SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        query = "select * from unidad_academica"
        Dim dataTable As DataTable = ConnectionOracle.SelectQuery(query)

        ComboBox1.DataSource = dataTable
        ComboBox1.DisplayMember = "nombre" ' Columna que se mostrará en el ComboBox
        ComboBox1.ValueMember = "id" '
        'SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        query = "SELECT distinct(GRADO) FROM AlumnosNormalizados"
        dataTable = ConnectionOracle.SelectQuery(query)

        cbogrado.DataSource = dataTable
        cbogrado.DisplayMember = "grado" ' Columna que se mostrará en el ComboBox
        cbogrado.ValueMember = "grado" '
        'SSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
        query = "SELECT distinct(anio) FROM AlumnosNormalizados"
        dataTable = ConnectionOracle.SelectQuery(query)

        cboyear.DataSource = dataTable
        cboyear.DisplayMember = "anio" ' Columna que se mostrará en el ComboBox
        cboyear.ValueMember = "anio"

        ComboBox1.SelectedIndex = 0
        cbogrado.SelectedIndex = 0
        cboyear.SelectedIndex = 0

    End Sub
    Private Sub lenguas()
        Me.Chart1.Series("n").Points.Clear()
        query = "select * from VWunidaddinamico where id = '" + idunidad + "' and anio = '" + anio + "' and grado = '" + Grado + "'"
        Dim dtnumalumnos As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtnumalumnos.Rows
            Dim n As String = row("lengua").ToString()
            Dim totalAlumnos As Integer = Convert.ToInt32(row("numalumnos"))

            Me.Chart1.Series("n").Points.AddXY(n, totalAlumnos)
            Me.Chart1.Series("n").Points(Me.Chart1.Series("n").Points.Count - 1).Label = totalAlumnos.ToString()
        Next
        forzarnombres(Chart1)
    End Sub
    Private Sub numalumnos()
        Me.Chart3.Series("Unidad").Points.Clear()
        query = "select * from VWunidad where anio = '" + anio + "'"
        Dim dtnumalumnos As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtnumalumnos.Rows
            Dim unidad As String = row("nombre").ToString()
            Dim totalAlumnos As Integer = Convert.ToInt32(row("numalumnos"))

            Me.Chart3.Series("Unidad").Points.AddXY(unidad, totalAlumnos)
            Me.Chart3.Series("Unidad").Points(Me.Chart3.Series("Unidad").Points.Count - 1).Label = totalAlumnos.ToString()
        Next
        forzarnombres(Chart3)
        idunidad = ComboBox1.SelectedValue.ToString()
        Label4.Text = "Distribución de Alumnos por Unidades (" + anio.ToString + ")"
    End Sub
    Private Sub nacion()
        Me.Chart4.Series("Nacion").Points.Clear()
        query = "select  nacion,numalumnos from VWunidadnacion where id = '" + idunidad + "' and anio = '" + anio + "'"
        Dim dtnacion As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtnacion.Rows
            Dim Nacion As String = row("Nacion").ToString()
            Dim totalAlumnos As Integer = Convert.ToInt32(row("numalumnos"))

            Me.Chart4.Series("Nacion").Points.AddXY(Nacion, totalAlumnos)
            Me.Chart4.Series("Nacion").Points(Me.Chart4.Series("Nacion").Points.Count - 1).Label = totalAlumnos.ToString()
        Next
        forzarnombres(Chart4)
        idunidad = ComboBox1.SelectedValue.ToString()
    End Sub
    Function forzarnombres(ByVal grafico As DataVisualization.Charting.Chart)
        ' Ajustar la visualización de los nombres en el eje X
        grafico.ChartAreas(0).AxisX.LabelStyle.Angle = -90 ' Rotar los nombres
        grafico.ChartAreas(0).AxisX.Interval = 1 ' Mostrar todos los nombres
        grafico.ChartAreas(0).AxisX.IntervalAutoMode = DataVisualization.Charting.IntervalAutoMode.FixedCount
        grafico.ChartAreas(0).AxisX.IsLabelAutoFit = True
    End Function

    Private Sub cboyear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboyear.SelectedIndexChanged
        anio = cboyear.SelectedValue.ToString()
        dinamicos()
    End Sub

    Private Sub cbogrado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbogrado.SelectedIndexChanged
        Grado = cbogrado.SelectedValue.ToString()
        dinamicos()
    End Sub

    Private Sub genero()
        Me.Chart2.Series("Genero").Points.Clear()
        query = "select  numalumnos, genero from VWunidaddinamicogenero where id = '" + idunidad + "' and anio = '" + anio + "' and grado ='" + Grado + "' "
        Dim dtgenero As DataTable = ConnectionOracle.SelectQuery(query)

        For Each row As DataRow In dtgenero.Rows
            Dim Genero As String = row("genero").ToString()
            Dim totalAlumnos As Integer = Convert.ToInt32(row("numalumnos"))

            Me.Chart2.Series("Genero").Points.AddXY(Genero, totalAlumnos)
            Me.Chart2.Series("Genero").Points(Me.Chart2.Series("Genero").Points.Count - 1).Label = totalAlumnos.ToString()
        Next
        forzarnombres(Chart2)
        idunidad = ComboBox1.SelectedValue.ToString()
    End Sub

    Private Sub Chart1_Click(sender As Object, e As EventArgs) Handles Chart1.Click

    End Sub

    Private Sub Chart4_Click(sender As Object, e As EventArgs) Handles Chart4.Click

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        idunidad = ComboBox1.SelectedValue.ToString()
        dinamicos()
    End Sub
    Private Sub dinamicos()
        lenguas()
        numalumnos()
        genero()
        nacion()
    End Sub
    Private Sub Chart3_Click_1(sender As Object, e As EventArgs) Handles Chart3.Click

    End Sub
End Class