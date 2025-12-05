Imports System.IO
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Services
Imports Google.Apis.Sheets.v4
Imports Google.Apis.Sheets.v4.Data
Imports Windows.Win32.System
Public Class Common

    ''' <summary>
    ''' GetGoogleSheetData
    ''' スプレッドシートデータ取得
    ''' </summary>
    Function GetGoogleSheetData(sheet_id As String, sheet_name As String, range As String) As IList(Of IList(Of Object))

        Dim service As SheetsService = Nothing
        Try
            ' 認証情報のロード（サービスアカウントキーのJSONファイル）
            ' ファイルパスは、環境に応じて適切に変更してください
            Dim credential As ServiceAccountCredential
            Using stream As New FileStream("credentials.json", FileMode.Open, FileAccess.Read)
                credential = GoogleCredential.FromStream(stream).
                    CreateScoped(New String() {SheetsService.Scope.SpreadsheetsReadonly}).
                    UnderlyingCredential
            End Using

            ' SheetsServiceの作成
            service = New SheetsService(New BaseClientService.Initializer() With {
                .HttpClientInitializer = credential,
                .ApplicationName = "Google Sheets API VB.NET Demo"
            })

            ' データの読み取りリクエストの作成
            Dim request As SpreadsheetsResource.ValuesResource.GetRequest =
                service.Spreadsheets.Values.Get(sheet_id, sheet_name & "!" & range)

            ' データの実行と取得
            Dim response As ValueRange = request.Execute()
            Dim values As IList(Of IList(Of Object)) = response.Values

            If values IsNot Nothing AndAlso values.Count > 0 Then
                ' データが正常に取得されました
                Console.WriteLine("スプレッドシートからデータを取得しました。")
                Return values
            Else
                Console.WriteLine("データが見つかりませんでした。")
                Return New List(Of IList(Of Object))()
            End If

        Catch ex As Exception
            Console.WriteLine($"エラーが発生しました: {ex.Message}")
            ' エラー処理を適切に行ってください
            Return New List(Of IList(Of Object))()
        End Try

    End Function
End Class
