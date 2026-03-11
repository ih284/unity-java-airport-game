package com.example.unitygatwick.domain.player;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/players")
@RequiredArgsConstructor
@CrossOrigin(origins = "*")
public class PlayerController {

    private final PlayerService playerService;
    private final PlayerRepository playerRepository;

    @PostMapping("/score")
    public Player saveScore(@RequestBody Player player) {
        return playerService.saveScore(player.getUsername(), player.getHighScore(), player.getGateNumber());
    }

    @GetMapping("/leaderboard/{gateNumber}")
    public List<Player> fetchTopTen(@PathVariable int gateNumber) {
        return playerService.getTop10Leaderboard(gateNumber);
    }

    @GetMapping("/count/{gateNumber}")
    public long countPlayer(@PathVariable int gateNumber){
        return playerRepository.countByGateNumber(gateNumber);
    }

    @DeleteMapping("/delete/{gateNumber}")
    public void deletePlayer(@PathVariable int gateNumber){
        //delete
    }


}
