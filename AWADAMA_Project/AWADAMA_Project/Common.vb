Imports System.IO
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Services
Imports Google.Apis.Sheets.v4
Imports Google.Apis.Sheets.v4.Data
Imports Windows.Win32.System
Public Module Common

    ''' <summary>
    ''' ListShuffle'''
    ''' </summary>
    ''' <returns></returns>
    Public Function ListShuffle(Of T)(ByVal list As List(Of T)) As List(Of T)
        Dim rng As New System.Random()

        Dim n As Integer = list.Count

        For i As Integer = n - 1 To 1 Step -1
            Dim j As Integer = rng.Next(0, i + 1)

            Dim temp As T = list(i)
            list(i) = list(j)
            list(j) = temp
        Next

        Return list
    End Function

#Region "スプレッドシート操作"

    ''' <summary>
    ''' GetGoogleSheetData
    ''' スプレッドシートデータ取得
    ''' </summary>
    Function GetGoogleSheetData(sheet_id As String, sheet_name As String, range As String) As IList(Of IList(Of Object))

        Dim service As SheetsService = Nothing
        Dim credential As ServiceAccountCredential
        Using stream As New FileStream("credentials.json", FileMode.Open, FileAccess.Read)
            credential = GoogleCredential.FromStream(stream).
                    CreateScoped(New String() {SheetsService.Scope.SpreadsheetsReadonly}).
                    UnderlyingCredential
        End Using

        service = New SheetsService(New BaseClientService.Initializer() With {
                .HttpClientInitializer = credential,
                .ApplicationName = "Google Sheets API VB.NET Demo"
            })

        Dim request As SpreadsheetsResource.ValuesResource.GetRequest =
                service.Spreadsheets.Values.Get(sheet_id, sheet_name & "!" & range)

        Dim response As ValueRange = request.Execute()
        Dim values As IList(Of IList(Of Object)) = response.Values

        If values IsNot Nothing AndAlso values.Count > 0 Then
            Console.WriteLine("スプレッドシートからデータを取得しました。")
            Return values
        Else
            Throw New Exception("スプレッドシートからデータを取得できませんでした。")
        End If

    End Function

    ''' <summary>
    ''' GetGoogleSheetData
    ''' スプレッドシートデータ入力（単一）
    ''' </summary>
    Sub SetGoogleSheetData(sheet_id As String, sheet_name As String, range As String, values As String)
        Dim valueList As IList(Of IList(Of Object))
        valueList = New List(Of IList(Of Object)) From {
            New List(Of Object) From {
                values
            }
        }

        SetGoogleSheetData(sheet_id, sheet_name, range, valueList)
    End Sub

    ''' <summary>
    ''' GetGoogleSheetData
    ''' スプレッドシートデータ入力（範囲）
    ''' </summary>
    Sub SetGoogleSheetData(sheet_id As String, sheet_name As String, range As String, values As IList(Of IList(Of Object)))

        Dim service As SheetsService = Nothing
        Dim credential As ServiceAccountCredential
        Using stream As New FileStream("credentials.json", FileMode.Open, FileAccess.Read)
            credential = GoogleCredential.FromStream(stream).
                CreateScoped(New String() {SheetsService.Scope.Spreadsheets}). ' ★ 読み取り専用ではないスコープ
                UnderlyingCredential
        End Using

        ' SheetsServiceの作成
        service = New SheetsService(New BaseClientService.Initializer() With {
            .HttpClientInitializer = credential,
            .ApplicationName = "Google Sheets API VB.NET Writer Demo"
        })

        Dim valueRange As New ValueRange()
        valueRange.Values = values

        Dim updateRequest As SpreadsheetsResource.ValuesResource.UpdateRequest =
            service.Spreadsheets.Values.Update(valueRange, sheet_id, sheet_name & "!" & range)

        updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED

        ' リクエストの実行
        Dim response As UpdateValuesResponse = updateRequest.Execute()

        Console.WriteLine($"スプレッドシートへの書き込みが完了しました。")
        Console.WriteLine($"更新されたセル数: {response.UpdatedCells}")

    End Sub

#End Region

End Module
