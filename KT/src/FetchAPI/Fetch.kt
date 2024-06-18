package FetchAPI

import java.io.File
import java.net.HttpURLConnection
import java.net.URL

fun fetchInput(day: Int, year: Int = 2018) {

    val session = File("session.txt").readText().trim()
    if(session.isEmpty()) {
        println("Please provide a session token in session.txt")
        return
    }

    val url = URL("https://adventofcode.com/$year/day/$day/input")
    val connection = url.openConnection() as HttpURLConnection

    connection.setRequestProperty("User-Agent", "Mozilla/5.0")
    connection.setRequestProperty("Cookie", "session=$session")

    connection.connect()

    val responseCode = connection.responseCode

    val content = connection.inputStream.bufferedReader().use {
        it.readText()
    }

    connection.disconnect()

    val path = "inputs/day$day.txt"
    println("Cached input to $path")
    File(path).writeText(content)
}

fun GetInput(day: Int, year: Int = 2018): List<String> {
    val path = "inputs/day$day.txt"

    if (!File(path).exists()) {
        fetchInput(day, year)
    }

    var input = File(path).readText().split('\n')

    input = input.dropLast(1)
    println("Fetched input for day $day from $path")
    return input
}