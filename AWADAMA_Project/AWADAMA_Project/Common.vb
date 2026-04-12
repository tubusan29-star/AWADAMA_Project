Imports System.IO
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Services
Imports Google.Apis.Sheets.v4
Imports Google.Apis.Sheets.v4.Data
Imports Npgsql
Imports Windows.Win32.System
Public Module Common

    Public Function GetCardDataByName(card_name As String) As CardDataDto
        For Each card As CardDataDto In AllCardData.Data
            If card.Name = card_name Then
                Return card
            End If
        Next
        Return New CardDataDto()
    End Function

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

    Async Function GetGoogleSheeDataAsync(sheet_id As String, sheet_name As String, range As String) As Task(Of IList(Of IList(Of Object)))
        Return Await Task.Run(Function() GetGoogleSheetData(sheet_id, sheet_name, range))
    End Function

    ''' <summary>
    ''' GetGoogleSheetData
    ''' スプレッドシートデータ取得
    ''' </summary>
    Function GetGoogleSheetData(sheet_id As String, sheet_name As String, range As String) As IList(Of IList(Of Object))

        Dim values As IList(Of IList(Of Object)) = New List(Of IList(Of Object))

        Try

            Dim request As SpreadsheetsResource.ValuesResource.GetRequest =
                        SheetsClient.GetService().Spreadsheets.Values.Get(sheet_id, sheet_name & "!" & range)

            Dim response As ValueRange = request.Execute()

            values = response.Values

        Catch ex As Exception

        End Try

        Return values

    End Function

    Async Sub SetGoogleSheetDataAsync(sheet_id As String, sheet_name As String, range As String, values As String)
        Await Task.Run(Sub() SetGoogleSheetData(sheet_id, sheet_name, range, values))
    End Sub

    Async Sub SetGoogleSheetDataAsync(sheet_id As String, sheet_name As String, range As String, values As IList(Of IList(Of Object)))
        Await Task.Run(Sub() SetGoogleSheetData(sheet_id, sheet_name, range, values))
    End Sub

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

        Try
            Dim valueRange As New ValueRange()
            valueRange.Values = values

            Dim updateRequest As SpreadsheetsResource.ValuesResource.UpdateRequest =
                SheetsClient.GetService().Spreadsheets.Values.Update(valueRange, sheet_id, sheet_name & "!" & range)

            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED

            ' リクエストの実行
            Dim response As UpdateValuesResponse = updateRequest.Execute()
        Catch ex As Exception

        End Try

    End Sub

#End Region

End Module

' VB.NET - SheetsService をキャッシュして再利用する例
Public Module SheetsClient
    Private _service As SheetsService = Nothing
    Private ReadOnly lockObj As New Object()

    Public Function GetService() As SheetsService
        If _service Is Nothing Then
            SyncLock lockObj
                If _service Is Nothing Then
                    Dim credential As ServiceAccountCredential
                    Using stream As New FileStream("credentials.json", FileMode.Open, FileAccess.Read)
                        credential = GoogleCredential.FromStream(stream).
                            CreateScoped(New String() {SheetsService.Scope.Spreadsheets}).
                            UnderlyingCredential
                    End Using
                    _service = New SheetsService(New BaseClientService.Initializer() With {
                        .HttpClientInitializer = credential,
                        .ApplicationName = "Google Sheets API VB.NET Demo"
                    })
                End If
            End SyncLock
        End If
        Return _service
    End Function

    ' 簡易バックオフ付きの実行ユーティリティ
    Public Function ExecuteWithBackoff(Of T)(action As Func(Of T)) As T
        Dim maxAttempts = 5
        Dim delayMs = 500
        For attempt = 1 To maxAttempts
            Try
                Return action()
            Catch ex As Google.GoogleApiException
                If ex.HttpStatusCode = Net.HttpStatusCode.TooManyRequests And attempt < maxAttempts Then
                    Threading.Thread.Sleep(delayMs)
                    delayMs *= 2
                    Continue For
                End If
                Throw
            End Try
        Next
        Return Nothing
    End Function
End Module
