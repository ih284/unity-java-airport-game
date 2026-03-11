package com.example.unitygatwick.domain.player;

import jakarta.transaction.Transactional;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@RequiredArgsConstructor
@Transactional
public class PlayerService {

    private final PlayerRepository playerRepository;

    public Player saveScore(String name, int score, int gateNumber) {
        Player player = playerRepository.findByUsername(name)
                .orElseGet(() -> {
            Player newP = new Player();
            newP.setUsername(name);
            newP.setGateNumber(gateNumber);
            return newP;
        });

        player.setGateNumber(gateNumber);

        // Only update when the highest score has been achieved
        if (score > player.getHighScore()) {
            player.setHighScore(score);
        }

        return playerRepository.save(player);
    }

    // Leaderboard Retrieve Top 10
    public List<Player> getTop10Leaderboard(int gateNumber) {
        return playerRepository.findTop10ByGateNumberOrderByHighScoreDesc(gateNumber);
    }
}
